using UnityEngine;

public class SkillMovement : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển của skill

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; // Di chuyển skill sang trái
    }
}
