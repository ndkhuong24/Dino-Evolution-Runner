using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            transform.position += Vector3.left * gameManager.speedScroller * Time.deltaTime;
        }

        // Xóa chướng ngại vật khi ra khỏi màn hình
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}
