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

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void ActionAnimator()
    {
        animator.SetTrigger("isActive");
    }
}
