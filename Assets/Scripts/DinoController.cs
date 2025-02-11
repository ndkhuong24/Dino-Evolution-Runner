using System;
using System.Collections;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 12.5f;
    private bool isGrounded = true;
    private bool isDashing = false;
    public Animator anim;

    //public bool isFacingRight = true;
    //public float move;
    //public float speed = 4.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Lấy Animator từ nhân vật
        rb.gravityScale = 3.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDashing)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)  // Nhấn Shift để Dash
        {
            Dash();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded)
        {
            isDashing = false;
            anim.SetBool("isDashing", false);
        }

        //di chuyển nhân vật
        //move = Input.GetAxisRaw("Horizontal");
        //rb.linearVelocity = new Vector2(speed * move, rb.linearVelocity.y);
        ////xoay nhân vật
        //if (isFacingRight == true && move == -1)
        //{
        //    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    isFacingRight = false;
        //}
        //else if (isFacingRight == false && move == 1)
        //{
        //    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    isFacingRight = true;
        //}
    }

    private void Dash()
    {
        isDashing = true;
        //isGrounded = false;
        anim.SetBool("isDashing", true);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        anim.SetBool("isJumping", true); // Bật animation Jump
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu chạm đất, có thể nhảy tiếp
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false); // Khi chạm đất, quay lại animation Run
        }

        if (collision.gameObject.CompareTag("Obstacle")) // Nếu va chạm với chướng ngại vật
        {
            FindFirstObjectByType<GameManager>().EndGame(); // Gọi hàm EndGame() trong GameManager
        }
    }
}
