using UnityEngine;

public class DinoController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float originalGravity; // Lưu trọng lực ban đầu

    [Header("Jump Settings")]
    public float jumpForce = 12.5f; // Lực nhảy tối ưu
    private bool isGrounded = true;

    [Header("Game Settings")]
    public float gravityScale = 2.5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalGravity = gravityScale; // Lưu trọng lực ban đầu
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();

        //anim.SetBool("isDashing", Input.GetKey(KeyCode.LeftShift) && isGrounded);
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

    // Bật-Tắt trọng lực khi đi qua portal
    public void SetGravity(bool enable)
    {
        if (enable)
            rb.gravityScale = originalGravity; 
        else
            rb.gravityScale = 0f;
    }
}