using UnityEngine;

public class DinoController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Jump Settings")]
    public float jumpForce = 12.5f; // Lực nhảy tối ưu
    private bool isGrounded = true;

    [Header("Game Settings")]
    public float gravityScale = 2.5f; // Trọng lực giống game gốc

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Nhấn Shift để cúi xuống
        //if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        //{
        //    anim.SetBool("isDashing", true);
        //}
        //else
        //{
        //    anim.SetBool("isDashing", false);
        //}
        bool isDashing = Input.GetKey(KeyCode.LeftShift) && isGrounded;
        anim.SetBool("isDashing", isDashing);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        anim.SetBool("isJumping", true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>().EndGame();
        }
    }
}
