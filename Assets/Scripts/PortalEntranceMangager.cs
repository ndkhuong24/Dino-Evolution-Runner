using System;
using UnityEngine;

public class PortalEntranceMangager : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.left * gameManager.globalSpeed * Time.fixedDeltaTime;

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ẩn Player khi chạm vào cổng
            collision.gameObject.SetActive(false);

            // Bật isTrigger của Player để tránh va chạm khác làm end game
            collision.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void ActionAnimator()
    {
        animator.SetTrigger("isActive");
    }
}
