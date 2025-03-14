using System;
using UnityEngine;

public class PortalGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f;
    private Animator animator;
    private float destroyBoundaryX; // Giới hạn để hủy đạn khi ra khỏi màn hình

    [Header("Portal Settings")]
    public GameObject portalEntrancePrefab; // Prefab của cổng vào
    public GameObject portalExitPrefab; // Prefab của cổng ra
    public float portalOffset = 1.5f; // Khoảng cách giữa cổng và vật cản

    private PortalEntranceMangager portalEntranceMangager;
    private PortalExitManager portalExitManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetTrigger("isShooting");

        destroyBoundaryX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 2f;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;

        if (transform.position.x > destroyBoundaryX)
        {
            Destroy(gameObject);
        }

        CheckObstacleDistance();
    }

    private void CheckObstacleDistance()
    {
        // Tìm tất cả các vật cản
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            // tinh khoang cach giua vi tri cua dan va vat can
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

            if (distance <= portalOffset)
            {
                CreatePortals(obstacle.transform.position);
                Destroy(gameObject);
                break;
            }
        }
    }

    private void CreatePortals(Vector3 obstaclePos)
    {
        Vector3 portalEntrancePos = obstaclePos - Vector3.right * portalOffset;
        Vector3 portalExitPos = obstaclePos + Vector3.right * portalOffset;

        // Instantiate Portal Entrance
        GameObject entranceObj = Instantiate(portalEntrancePrefab, portalEntrancePos, Quaternion.identity);
        PortalEntranceMangager entranceManager = entranceObj.GetComponent<PortalEntranceMangager>();
        if (entranceManager != null)
        {
            entranceManager.ActionAnimator();
        }
        else
        {
            Debug.LogError("PortalEntranceMangager is missing on Portal Entrance prefab!");
        }

        // Instantiate Portal Exit
        GameObject exitObj = Instantiate(portalExitPrefab, portalExitPos, Quaternion.identity);
        PortalExitManager exitManager = exitObj.GetComponent<PortalExitManager>();
        if (exitManager != null)
        {
            exitManager.ActionAnimator();
        }
        else
        {
            Debug.LogError("PortalExitManager is missing on Portal Exit prefab!");
        }
    }

}
