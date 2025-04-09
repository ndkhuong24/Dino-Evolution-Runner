using System;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float originalGravity;

    [Header("Jump Settings")]
    public float jumpForce = 12.5f;
    private bool isGrounded = true;

    [Header("Game Settings")]
    public float gravityScale = 2.5f;

    public bool isInPortal = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        originalGravity = gravityScale;
        rb.gravityScale = gravityScale;

        ResetState();
    }

    void Update()
    {
        if (isInPortal) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true) Jump();
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) > 0.01f) return;

        isGrounded = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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

    public void SetGravity(bool enable)
    {
        if (enable)
            rb.gravityScale = originalGravity;
        else
            rb.gravityScale = 0f;
    }

    public void SetInPortal(bool state)
    {
        isInPortal = state;
    }

    private void ResetState()
    {
        isGrounded = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isJumping", false);
        rb.gravityScale = originalGravity;
        isInPortal = false;
    }
}