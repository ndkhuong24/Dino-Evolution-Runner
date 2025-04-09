using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (gameManager == null || gameManager.globalSpeed <= 0) return;

        transform.position += Vector3.left * gameManager.globalSpeed * Time.deltaTime;

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}