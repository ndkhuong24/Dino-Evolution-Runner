using UnityEngine;

public class SkillMovement : MonoBehaviour
{
    public float speed = 4f; // Tốc độ di chuyển của skill

    void Start()
    {

    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Nếu Skill đi quá giới hạn bên trái, tự hủy
        if (transform.position.x < -10f) // Giả sử -10f là giới hạn bên trái
        {
            Destroy(gameObject);
        }
    }

}
