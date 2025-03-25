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
        if (transform.position.x < -10f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DinoController playerController = collision.GetComponent<DinoController>();
            Collider2D col = collision.GetComponent<Collider2D>();

            if (playerController != null && col != null)
            {
                playerController.SetGravity(false); // Tắt trọng lực
                col.isTrigger = true;              // Tránh va chạm
                //collision.GetComponent<SpriteRenderer>().enabled = false;

                SpriteRenderer[] renderers = collision.GetComponentsInChildren<SpriteRenderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = false;
                }
            }
        }
    }

    public void ActionAnimator()
    {
        animator.SetTrigger("isActive");
    }
}