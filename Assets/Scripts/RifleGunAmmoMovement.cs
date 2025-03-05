using UnityEngine;

public class RifleGunAmmoMovement : MonoBehaviour
{
    public float speed = 10f; // Tốc độ di chuyển của đạn

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 10f) // ✅ Hủy đạn khi nó đi quá xa bên phải màn hình
        {
            Destroy(gameObject);
        }
    }
}