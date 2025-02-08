using UnityEngine;

public class DinoController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
}
