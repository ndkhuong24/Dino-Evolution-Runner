using UnityEngine;

public class SkillMovement : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < -10f) ObjectPool.Instance.ReturnObject(gameObject);
        //if (transform.position.x < -10f) Destroy(gameObject);
    }
}