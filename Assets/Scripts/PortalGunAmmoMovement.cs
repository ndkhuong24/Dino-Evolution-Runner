using UnityEngine;

public class PortalGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f;
    public Vector3 direction = Vector3.right; // Hướng di chuyển của đạn (mặc định là sang phải)
    private Animator animator;
    private float destroyBoundaryX; // Giới hạn để hủy đạn khi ra khỏi màn hình

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetTrigger("isShooting");

        destroyBoundaryX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;

        // Kiểm tra nếu viên đạn ra khỏi giới hạn màn hình thì hủy nó
        if (transform.position.x > destroyBoundaryX)
        {
            Destroy(gameObject);
        }
    }
}
