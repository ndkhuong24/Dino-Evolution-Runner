using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class PooledObstacle : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        // Khi được lấy ra từ pool, luôn reset trạng thái
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        if (col != null)
            col.isTrigger = false;

        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}