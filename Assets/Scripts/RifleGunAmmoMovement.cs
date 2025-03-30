using UnityEngine;

public class RifleGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f; 

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 10f) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) 
        {
            Transform parent = other.transform.parent; 

            if (parent != null && parent.name == "ObstacleGroup")
            {
                Destroy(parent.gameObject); 
            }
            else
            {
                Destroy(other.gameObject); 
            }

            Destroy(gameObject); 
        }
    }
}