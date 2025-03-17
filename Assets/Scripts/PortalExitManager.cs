using UnityEngine;

public class PortalExitManager : MonoBehaviour
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
                col.isTrigger = false;               // Cho phép va chạm lại
                playerController.SetGravity(true);   // Bật trọng lực trở lại
                //collision.GetComponent<SpriteRenderer>().enabled = true;

                SpriteRenderer[] renderers = collision.GetComponentsInChildren<SpriteRenderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = true;
                }
            }
        }
    }

    public void ActionAnimator()
    {
        animator.SetTrigger("isActive");
    }
}
