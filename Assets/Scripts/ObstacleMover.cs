using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager không tìm thấy trong scene!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameManager == null || gameManager.speedScroller <= 0) return;

        transform.position += Vector3.left * gameManager.speedScroller * Time.deltaTime;

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}
