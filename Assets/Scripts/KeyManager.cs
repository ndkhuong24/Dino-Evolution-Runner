using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    [Header("KeySetting")]
    public string keyName;
    private CanvasGroup keyCanvasGroup;
    private CanvasGroup skillCanvasGroup;
    private Image skillIconImage;

    [Header("AmmoSetting")]
    public TextMeshProUGUI ammoText;
    private int currentAmmo;
    private Skill assignedSkill = null;
    public TextMeshProUGUI stealthTime;

    [Header("SkillSetting")]
    private RifleGunController rifleGunController;
    private PortalGunController portalGunController;

    private bool isStealthActive = false;
    private GameObject player;
    private Collider2D playerCollider;
    private Text StealthTimer;

    private void Awake()
    {
        keyCanvasGroup = GetComponent<CanvasGroup>();
        skillCanvasGroup = transform.Find("SkillIcon").GetComponent<CanvasGroup>();
        skillIconImage = transform.Find("SkillIcon").GetComponent<Image>();

        player = GameObject.Find("Player");
        if (player != null) playerCollider = player.GetComponent<Collider2D>();

        if (player != null)
        {
            rifleGunController = player.transform.Find("RifleGun")?.GetComponent<RifleGunController>();
            portalGunController = player.transform.Find("PortalGun")?.GetComponent<PortalGunController>();
        }
    }

    public bool HasSkill() => assignedSkill != null;

    internal void SetSkill(Skill skill)
    {
        assignedSkill = skill;
        currentAmmo = skill.skillCost;
        keyCanvasGroup.alpha = 1f;
        skillCanvasGroup.alpha = 1f;
        skillIconImage.sprite = skill.icon;
    }

    internal void ActivateSkill()
    {
        if (assignedSkill == null || player == null) return;

        switch (assignedSkill.skillName)
        {
            case "PortalSkill":
                if (currentAmmo > 0)
                {
                    SwitchWeapon(portalGunController);
                    portalGunController.FireAmmo();
                    UseAmmo();
                }
                break;
            case "RifleSkill":
                if (currentAmmo > 0)
                {
                    SwitchWeapon(rifleGunController);
                    rifleGunController.ShootAnimation();
                    rifleGunController.FireAmmo();
                    UseAmmo();
                }
                break;
            case "StealthSkill":
                if (!isStealthActive)
                {
                    StartCoroutine(ActivateStealthSkill());
                    ResetSkill();
                }
                break;
        }
    }

    private void UseAmmo()
    {
        currentAmmo--;
        UpdateAmmoText();
        if (currentAmmo == 0) Invoke(nameof(ResetSkill), 0.2f);
    }

    private void SwitchWeapon(MonoBehaviour weapon)
    {
        rifleGunController?.DeactivateWeapon();
        portalGunController?.DeactivateWeapon();
        weapon?.gameObject.SetActive(true);
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentAmmo > 0 ? $"Ammo: {currentAmmo}" : "";
    }

    private IEnumerator ActivateStealthSkill()
    {
        isStealthActive = true;
        float skillDuration = 10f;
        float remaining = skillDuration;

        while (remaining > 0)
        {
            ApplyStealthEffect();
            stealthTime.text = $"{remaining:F1}s";
            yield return new WaitForSeconds(0.1f);
            remaining -= 0.1f;
        }

        stealthTime.text = "";
        isStealthActive = false;

        //Đợi cho người chơi ra khỏi vật cản
        while (IsPlayerInsideObstacle() || IsObstacleInFront())
        {
            yield return new WaitForSeconds(0.5f);
          //elapsedTime += 0.5f;
        }

        ResetStealthEffect();
    }

    private bool IsObstacleInFront()
    {
        return Physics2D.Raycast(player.transform.position, Vector2.right, 1f, LayerMask.GetMask("StealthObstacle"));
    }

    private bool IsPlayerInsideObstacle()
    {
        return Physics2D.OverlapBox(player.transform.position, new Vector2(1f, 1f), 0f, LayerMask.GetMask("StealthObstacle"));
    }

    private void ApplyStealthEffect()
    {
        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            var sr = obstacle.GetComponent<SpriteRenderer>();
            var col = obstacle.GetComponent<Collider2D>();

            if (sr != null) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            if (col != null) col.isTrigger = true;

            if (obstacle.layer != LayerMask.NameToLayer("StealthObstacle"))
            {
                obstacle.layer = LayerMask.NameToLayer("StealthObstacle");
            }
        }
    }

    private void ResetStealthEffect()
    {
        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            var sr = obstacle.GetComponent<SpriteRenderer>();
            var col = obstacle.GetComponent<Collider2D>();

            if (sr != null) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            if (col != null) col.isTrigger = false;

            if (obstacle.layer == LayerMask.NameToLayer("StealthObstacle"))
            {
                obstacle.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void ResetSkill()
    {
        if (assignedSkill != null)
        {
            if (assignedSkill.skillName == "PortalSkill")
            {
                portalGunController?.DeactivateWeapon();
            }
            else if (assignedSkill.skillName == "RifleSkill")
            {
                rifleGunController?.ResetShootAnimation();
                rifleGunController?.DeactivateWeapon();
            }
        }

        assignedSkill = null;
        currentAmmo = 0;
        keyCanvasGroup.alpha = 0.3f;
        skillCanvasGroup.alpha = 0f;
        skillIconImage.sprite = null;
    }
}