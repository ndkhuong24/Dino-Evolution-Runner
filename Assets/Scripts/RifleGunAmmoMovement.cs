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
            Destroy(other.gameObject); // Hủy vật cản
            Destroy(gameObject); // Hủy đạn
        }
    }
}
