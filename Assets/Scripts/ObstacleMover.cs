using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            //Debug.LogError("GameManager không tìm thấy trong scene!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameManager == null || gameManager.globalSpeed <= 0) return;

        transform.position += Vector3.left * gameManager.globalSpeed * Time.deltaTime;  // ✅ Dùng globalSpeed để đồng bộ

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}
