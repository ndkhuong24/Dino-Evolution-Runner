using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public string keyName; // Tên phím (Q, W, E, ...)
    private CanvasGroup keyCanvasGroup;
    private CanvasGroup skillCanvasGroup;
    private Image skillIconImage;

    public TextMeshProUGUI ammoText;

    private int currentAmmo;
    private Skill assignedSkill = null;

    private RifleGunController rifleGunController;
    private PortalGunController portalGunController;

    private bool isStealthActive = false; // Biến kiểm tra kỹ năng StealthSkill có đang hoạt động không

    private void Awake()
    {
        keyCanvasGroup = GetComponent<CanvasGroup>();
        skillCanvasGroup = transform.Find("SkillIcon").GetComponent<CanvasGroup>();
        skillIconImage = transform.Find("SkillIcon").GetComponent<Image>();
    }

    public bool HasSkill()
    {
        return assignedSkill != null;
    }

    internal void SetSkill(Skill skill)
    {
        assignedSkill = skill;
        currentAmmo = skill.skillCost; // Đặt số lần sử dụng ban đầu

        keyCanvasGroup.alpha = 1f;
        skillCanvasGroup.alpha = 1f;
        skillIconImage.sprite = skill.icon;
    }

    internal void ActivateSkill()
    {
        if (assignedSkill == null) return;

        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        switch (assignedSkill.skillName)
        {
            case "PortalSkill":
                if (currentAmmo > 0)
                {
                    if (portalGunController == null)
                    {
                        portalGunController = player.transform.Find("PortalGun").GetComponent<PortalGunController>();
                    }

                    portalGunController.ActivateWeapon();
                    portalGunController.FireAmmo();

                    currentAmmo--;
                    UpdateAmmoText();

                    if (currentAmmo == 0)
                    {
                        Invoke(nameof(ResetSkill), 0.2f);
                    }
                }
                break;
            case "RifleSkill":
                if (currentAmmo > 0)
                {
                    if (rifleGunController == null)
                    {
                        rifleGunController = player.transform.Find("RifleGun").GetComponent<RifleGunController>();
                    }

                    rifleGunController.ActivateWeapon();
                    rifleGunController.ShootAnimation();
                    rifleGunController.FireAmmo();

                    currentAmmo--;
                    UpdateAmmoText();

                    if (currentAmmo == 0)
                    {
                        Invoke(nameof(ResetSkill), 0.2f);
                    }
                }
                break;
            case "StealthSkill":
                if (isStealthActive) return; // Ngăn chặn kích hoạt lại khi đang tàng hình
                StartCoroutine(ActivateStealthSkill());
                ResetSkill();
                break;
        }
    }

    private void UpdateAmmoText()
    {
        if (currentAmmo > 0)
        {
            ammoText.text = "Ammo: " + currentAmmo.ToString();
        }
        else
        {
            ammoText.text = "";
        }
    }

    private IEnumerator ActivateStealthSkill()
    {
        isStealthActive = true; // Kích hoạt trạng thái tàng hình
        float elapsedTime = 0f;
        float skillDuration = 6f;

        while (elapsedTime < skillDuration)
        {
            ApplyStealthEffect(); // Áp dụng hiệu ứng lên tất cả vật cản hiện tại
            yield return new WaitForSeconds(0.1f); // Kiểm tra lại mỗi 0.1s
            elapsedTime += 0.1f;
        }

        isStealthActive = false; // Hết hiệu lực

        if (elapsedTime > skillDuration)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Collider2D playerCollider = player.GetComponent<Collider2D>();
                if (playerCollider != null)
                {
                    GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
                    bool stillTouching = false;

                    foreach (GameObject obstacle in obstacles)
                    {
                        Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
                        if (obstacleCollider != null && obstacleCollider.isTrigger && playerCollider.IsTouching(obstacleCollider))
                        {
                            stillTouching = true;
                            break;
                        }
                    }

                    if (stillTouching)
                    {
                        yield return new WaitForSeconds(0.5f); // Đợi thêm 0.5s rồi kiểm tra lại
                        elapsedTime -= 0.5f; // Giảm thời gian lại để tiếp tục vòng lặp
                    }
                    else
                    {
                        ResetStealthEffect(); // Nếu không còn chạm thì khôi phục trạng thái
                    }
                }
            }
        }
    }

    private void ApplyStealthEffect()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
            Collider2D col = obstacle.GetComponent<Collider2D>();

            if (sr != null)
            {
                Color newColor = sr.color;
                newColor.a = 0.5f; // Giảm độ alpha
                sr.color = newColor;
            }

            if (col != null)
            {
                col.isTrigger = true; // Cho phép nhân vật đi xuyên qua
            }
        }
    }

    private void ResetStealthEffect()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
            Collider2D col = obstacle.GetComponent<Collider2D>();

            if (sr != null)
            {
                Color newColor = sr.color;
                newColor.a = 1f; // Khôi phục alpha ban đầu
                sr.color = newColor;
            }

            if (col != null)
            {
                col.isTrigger = false; // Khôi phục va chạm
            }
        }
    }

    private void ResetSkill()
    {
        if (assignedSkill != null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                if (assignedSkill.skillName == "PortalSkill" && portalGunController != null)
                {
                    portalGunController.DeactivateWeapon();
                }
                else if (assignedSkill.skillName == "RifleSkill" && rifleGunController != null)
                {
                    rifleGunController.ResetShootAnimation();
                    rifleGunController.DeactivateWeapon();
                }
            }
        }

        assignedSkill = null;
        currentAmmo = 0;
        keyCanvasGroup.alpha = 0.3f;
        skillCanvasGroup.alpha = 0f;
        skillIconImage.sprite = null;
    }
}
