using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public string keyName; // Key name (Q, W, E, etc.)
    private CanvasGroup keyCanvasGroup; // Controls transparency of the key
    private CanvasGroup skillCanvasGroup; // Controls transparency of the skill icon
    private Image skillIconImage; // Skill icon image 

    private int currentAmmo;
    private Skill assignedSkill = null; // The currently assigned skill

    private void Awake()
    {
        if (keyCanvasGroup == null) keyCanvasGroup = GetComponent<CanvasGroup>();
        if (skillCanvasGroup == null) skillCanvasGroup = transform.Find("SkillIcon").GetComponent<CanvasGroup>();
        if (skillIconImage == null) skillIconImage = transform.Find("SkillIcon").GetComponent<Image>();
    }

    public bool HasSkill()
    {
        return assignedSkill != null;
    }

    internal void SetSkill(Skill skill)
    {
        assignedSkill = skill;
        currentAmmo = skill.skillCost; // Gán số lần sử dụng ban đầu

        keyCanvasGroup.alpha = 1f;
        skillCanvasGroup.alpha = 1f;
        skillIconImage.sprite = skill.icon;
    }

    internal void ActivateSkill()
    {
        if (assignedSkill == null)
        {
            return;
        }

        GameObject player = GameObject.Find("Player"); // Tìm Player

        if (player == null)
        {
            return;
        }

        switch (assignedSkill.skillName)
        {
            case "PortalSkill":
            case "RifleSkill":
                if (currentAmmo > 0)
                {
                    currentAmmo--;

                    ActivateWeapon(player, assignedSkill.skillName == "PortalSkill" ? "PortalGun" : "RifleGun");

                    if (currentAmmo == 0)
                    {
                        ResetSkill();
                    }
                }
                break;
            case "StealthSkill":
                ResetSkill();
                break;
            default:
                break;
        }
    }

    private void ResetSkill()
    {
        if (assignedSkill != null) {
            GameObject player = GameObject.Find("Player");
            if(player!=null)
            {
                if (assignedSkill.skillName == "PortalSkill")
                {
                    DeactivateWeapon(player, "PortalGun");
                }
                else if (assignedSkill.skillName == "RifleSkill")
                {
                    DeactivateWeapon(player, "RifleGun");
                }
            }
        }

        assignedSkill = null;
        currentAmmo = 0;
        keyCanvasGroup.alpha = 0.3f;
        skillCanvasGroup.alpha = 0f;
        skillIconImage.sprite = null;
    }

    private void DeactivateWeapon(GameObject player, string v)
    {
       Transform weaponTransform = player.transform.Find(v);
        if (weaponTransform != null)
        {
            weaponTransform.gameObject.SetActive(false);
        }
    }

    private void ActivateWeapon(GameObject player, string weaponName)
    {
        Transform weaponTransform = player.transform.Find(weaponName);

        if (weaponTransform != null)
        {
            weaponTransform.gameObject.SetActive(true);
        }
    }
}
