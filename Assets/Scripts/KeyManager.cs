using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public string keyName; // Tên phím (Q, W, E, ...)
    private CanvasGroup keyCanvasGroup;
    private CanvasGroup skillCanvasGroup;
    private Image skillIconImage;

    private int currentAmmo;
    private Skill assignedSkill = null;

    private RifleGunController rifleGunController;
    private PortalGunController portalGunController;

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
                    currentAmmo--;

                    if (portalGunController == null)
                    {
                        portalGunController = player.transform.Find("PortalGun").GetComponent<PortalGunController>();
                    }

                    portalGunController.ActivateWeapon();

                    if (currentAmmo == 0)
                    {
                        ResetSkill();
                    }
                }
                break;
            case "RifleSkill":
                if (currentAmmo > 0)
                {
                    currentAmmo--;

                    if (rifleGunController == null)
                    {
                        rifleGunController = player.transform.Find("RifleGun").GetComponent<RifleGunController>();
                    }
                    rifleGunController.ActivateWeapon();
                    rifleGunController.ShootAnimation();
                    rifleGunController.FireAmmo();
                    if (currentAmmo == 0)
                    {
                        ResetSkill();
                    }
                }
                break;
            case "StealthSkill":
                ResetSkill();
                break;
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
