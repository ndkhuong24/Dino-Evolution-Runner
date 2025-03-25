using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameObject obstacleGroup;

    public void SetObstacleGroup(GameObject group)
    {
        obstacleGroup = group;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ammo"))
        {
            DestroyObstacle();
        }
    }

    public void DestroyObstacle()
    {
        if (obstacleGroup != null)
        {
            Destroy(obstacleGroup);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}