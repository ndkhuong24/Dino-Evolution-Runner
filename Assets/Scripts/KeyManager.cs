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
    public TextMeshProUGUI stealthTime;
    private int currentAmmo;
    private Skill assignedSkill = null;

    [Header("SkillSetting")]
    private RifleGunController rifleGunController;
    private PortalGunController portalGunController;

    private bool isStealthActive = false;
    private bool waitingToReset = false;
    private bool stealthEffectApplied = false;

    private float stealthTimer = 0f;
    private float skillDuration = 10f;

    private GameObject player;
    private Collider2D playerCollider;

    private void Awake()
    {
        keyCanvasGroup = GetComponent<CanvasGroup>();
        skillCanvasGroup = transform.Find("SkillIcon").GetComponent<CanvasGroup>();
        skillIconImage = transform.Find("SkillIcon").GetComponent<Image>();

        player = GameObject.Find("Player");

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            rifleGunController = player.transform.Find("RifleGun")?.GetComponent<RifleGunController>();
            portalGunController = player.transform.Find("PortalGun")?.GetComponent<PortalGunController>();
        }
    }

    private void Update()
    {
        if (isStealthActive)
        {
            if (!stealthEffectApplied)
            {
                ApplyStealthEffect();
                stealthEffectApplied = true;
            }

            if (stealthTimer > 0f)
            {
                ApplyStealthEffect(); // Giữ hiệu ứng liên tục nếu cần
                stealthTimer -= Time.deltaTime;
                stealthTime.text = $"{stealthTimer:F1}s";
            }
            else
            {
                stealthTime.text = "";
                isStealthActive = false;
                waitingToReset = true;
            }
        }
        else if (waitingToReset)
        {
            if (!IsPlayerInsideObstacle() && !IsObstacleInFront())
            {
                ResetStealthEffect();
                waitingToReset = false;
                stealthEffectApplied = false;
                ResetSkill(); // Reset khi hoàn tất
            }
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
                ActivateStealthSkill();
                HideSkillDisplay(); // Ẩn giao diện ngay khi kích hoạt
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

    private void ActivateStealthSkill()
    {
        // Reset trạng thái để chạy lần mới
        isStealthActive = true;
        stealthTimer = skillDuration; // Reset thời gian về 10f
        stealthTime.text = $"{stealthTimer:F1}s"; // Cập nhật UI ngay lập tức
        waitingToReset = false; // Hủy trạng thái đợi nếu có

        // Nếu hiệu ứng đã áp dụng trước đó, không cần áp dụng lại
        if (!stealthEffectApplied)
        {
            ApplyStealthEffect();
            stealthEffectApplied = true;
        }
    }

    private void HideSkillDisplay()
    {
        // Chỉ ẩn giao diện skill, không reset toàn bộ
        keyCanvasGroup.alpha = 0.3f;
        skillCanvasGroup.alpha = 0f;
        skillIconImage.sprite = null;
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
        foreach (var obstacle in GameObject.FindObjectsByType<StealthSkillBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            var behavior = obstacle.GetComponent<StealthSkillBehavior>();
            if (behavior != null) behavior.Activate(player);
        }
    }

    private void ResetStealthEffect()
    {
        foreach (var obstacle in GameObject.FindObjectsByType<StealthSkillBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            var behavior = obstacle.GetComponent<StealthSkillBehavior>();
            if (behavior != null) behavior.Deactivate(player);
        }
    }

    //private void ApplyStealthEffect()
    //{
    //    foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
    //    {
    //        var sr = obstacle.GetComponent<SpriteRenderer>();
    //        var col = obstacle.GetComponent<Collider2D>();

    //        if (sr != null) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
    //        if (col != null) col.isTrigger = true;

    //        if (obstacle.layer != LayerMask.NameToLayer("StealthObstacle"))
    //        {
    //            obstacle.layer = LayerMask.NameToLayer("StealthObstacle");
    //        }
    //    }
    //}

    //private void ResetStealthEffect()
    //{
    //    foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
    //    {
    //        var sr = obstacle.GetComponent<SpriteRenderer>();
    //        var col = obstacle.GetComponent<Collider2D>();

    //        if (sr != null) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    //        if (col != null) col.isTrigger = false;

    //        if (obstacle.layer == LayerMask.NameToLayer("StealthObstacle"))
    //        {
    //            obstacle.layer = LayerMask.NameToLayer("Default");
    //        }
    //    }
    //}

    private void ResetSkill()
    {
        if (assignedSkill != null)
        {
            switch (assignedSkill.skillName)
            {
                case "PortalSkill":
                    portalGunController?.DeactivateWeapon();
                    break;
                case "RifleSkill":
                    rifleGunController?.ResetShootAnimation();
                    rifleGunController?.DeactivateWeapon();
                    break;
            }
        }
        //if (assignedSkill != null)
        //{
        //    if (assignedSkill.skillName == "PortalSkill")
        //    {
        //        portalGunController?.DeactivateWeapon();
        //    }
        //    else if (assignedSkill.skillName == "RifleSkill")
        //    {
        //        rifleGunController?.ResetShootAnimation();giờ
        //        rifleGunController?.DeactivateWeapon();
        //    }
        //}

        assignedSkill = null;
        currentAmmo = 0;

        keyCanvasGroup.alpha = 0.3f;
        skillCanvasGroup.alpha = 0f;
        skillIconImage.sprite = null;
    }

    private void OnDisable()
    {
        ResetStealthState();
    }

    private void ResetStealthState()
    {
        isStealthActive = false;
        waitingToReset = false;
        stealthEffectApplied = false;
        stealthTimer = 0f;
        stealthTime.text = "";
    }
}