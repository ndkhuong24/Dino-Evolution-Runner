using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StealthSkillBehavior : MonoBehaviour, ISkillBehavior
{
    private SpriteRenderer spriteRenderer;
    private Collider2D obstacleCollider;
    private bool isStealthed = false;

    private float originalAlpha;
    private bool? initialColliderState = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        obstacleCollider = GetComponent<Collider2D>();

        if (spriteRenderer != null) originalAlpha = spriteRenderer.color.a;

        if (obstacleCollider != null) initialColliderState = obstacleCollider.enabled;
    }

    public void Activate(GameObject user)
    {
        if (isStealthed) return;

        isStealthed = true;

        // Làm mờ object
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0.3f;
            spriteRenderer.color = color;
        }

        // Tắt va chạm
        if (obstacleCollider != null && obstacleCollider.enabled) obstacleCollider.enabled = false;
    }

    public void Deactivate(GameObject user)
    {
        if (!isStealthed) return;

        isStealthed = false;

        // Khôi phục độ mờ
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = originalAlpha;
            spriteRenderer.color = color;
        }

        // Bật va chạm
        if (obstacleCollider != null && initialColliderState == true) obstacleCollider.enabled = true;
    }

    public void ResetToDefault()
    {
        isStealthed = false;

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = originalAlpha;
            spriteRenderer.color = color;
        }

        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = true; // hoặc originalColliderState nếu bạn lưu
        }
    }
}