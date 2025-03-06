using System;
using System.Collections;
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

    private GameObject[] obstacles; // Lưu danh sách các chướng ngại vật

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
                    currentAmmo--;

                    if (currentAmmo == 0)
                    {
                        Invoke(nameof(ResetSkill), 0.2f); // Trì hoãn reset để đảm bảo hiệu ứng hiển thị
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

                    if (currentAmmo == 0)
                    {
                        Invoke(nameof(ResetSkill), 0.2f); // Trì hoãn reset để đảm bảo viên đạn cuối cùng xuất hiện
                    }
                }
                break;
            case "StealthSkill":
                StartCoroutine(ActivateStealthSkill());
                ResetSkill();
                break;
        }
    }

    private IEnumerator ActivateStealthSkill()
    {
        // Lấy danh sách tất cả các chướng ngại vật trên màn hình
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

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

        yield return new WaitForSeconds(5f); // Thời gian hiệu ứng kéo dài

        // Khôi phục trạng thái ban đầu
        foreach (GameObject obstacle in obstacles)
        {
            SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
            Collider2D col = obstacle.GetComponent<Collider2D>();

            if (sr != null)
            {
                Color newColor = sr.color;
                newColor.a = 1f; // Khôi phục độ alpha ban đầu
                sr.color = newColor;
            }

            if (col != null)
            {
                col.isTrigger = false; // Khôi phục lại va chạm
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
                    rifleGunController.ResetShootAnimation(); // Đảm bảo hoạt ảnh bắn súng tắt đúng cách
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