using UnityEngine;

public class PortalEntranceMangager : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;

    [HideInInspector] public Transform linkedPortalExit;

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
                playerController.SetGravity(false);
                col.isTrigger = true;
                playerController.SetInPortal(true);

                SpriteRenderer[] renderers = collision.GetComponentsInChildren<SpriteRenderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = false;
                }
            }
        }
        else if (collision.CompareTag("Ammo") && linkedPortalExit != null)
        {
            // Di chuyển Ammo sang đúng vị trí của Portal Exit mà không tăng tốc
            collision.transform.position = linkedPortalExit.position + Vector3.right * 0.5f;
        }
    }

    public void ActionAnimator()
    {
        animator.SetTrigger("isActive");
    }
}