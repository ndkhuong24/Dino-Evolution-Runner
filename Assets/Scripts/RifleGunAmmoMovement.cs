using UnityEngine;

public class RifleGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f; // Tốc độ di chuyển của đạn

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 10f) // ✅ Hủy đạn khi nó đi quá xa bên phải màn hình
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) // Kiểm tra nếu va chạm với Obstacle
        {
            Transform parent = other.transform.parent; // Kiểm tra có parent không

            if (parent != null && parent.name == "ObstacleGroup")
            {
                Destroy(parent.gameObject); // Nếu thuộc group, hủy toàn bộ group
            }
            else
            {
                Destroy(other.gameObject); // Nếu không thuộc group, chỉ hủy object
            }

            Destroy(gameObject); // Hủy đạn
        }
    }
}