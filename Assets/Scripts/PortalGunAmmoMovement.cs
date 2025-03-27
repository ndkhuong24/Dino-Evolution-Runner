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
    public float portalOffsetGroup = 2.5f; // Khoảng cách giữa cổng và nhóm vật cản

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

        //CheckObstacleDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform parentGroup = collision.transform.parent;

        //Kiểm tra xem có Group hay không
        if (parentGroup != null && parentGroup.CompareTag("ObstacleGroup"))
        {
            CreatePortals(parentGroup);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            CreatePortals(collision.transform);
            Destroy(gameObject);
        }
    }

    private void CreatePortals(Transform target)
    {
        Vector3 portalEntrancePos;
        Vector3 portalExitPos;

        if (target.CompareTag("ObstacleGroup"))
        {
            //Tìm vị trí trung tâm của Group
            Bounds groupBounds = GetGroupBounds(target);
            portalEntrancePos = groupBounds.center - Vector3.right * portalOffsetGroup;
            portalExitPos = groupBounds.center + Vector3.right * portalOffsetGroup;
        }
        else
        {
            //Nếu chỉ có một Obstacle, đặt portal như cũ
            portalEntrancePos = target.position - Vector3.right * portalOffset;
            portalExitPos = target.position + Vector3.right * portalOffset;
        }

        //Tạo cổng vào
        GameObject entranceObj = Instantiate(portalEntrancePrefab, portalEntrancePos, Quaternion.identity);
        portalEntranceMangager = entranceObj.GetComponent<PortalEntranceMangager>();
        if (portalEntranceMangager != null) portalEntranceMangager.ActionAnimator();

        //Tạo cổng ra
        GameObject exitObj = Instantiate(portalExitPrefab, portalExitPos, Quaternion.identity);
        portalExitManager = exitObj.GetComponent<PortalExitManager>();
        if (portalExitManager != null) portalExitManager.ActionAnimator();
    }

    private Bounds GetGroupBounds(Transform target)
    {
        Bounds bounds = new Bounds(target.position, Vector3.zero);

        foreach (Transform child in target)
        {
            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null)
            {
                bounds.Encapsulate(col.bounds);
            }
        }

        return bounds;
    }

    //Code mới
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Obstacle") || collision.CompareTag("ObstacleGroup"))
    //    {
    //        CreatePortals(collision.transform.position);
    //        Destroy(gameObject);
    //    }
    //}

    //private void CreatePortals(Vector3 obstaclePos)
    //{
    //    Vector3 portalEntrancePos = obstaclePos - Vector3.right * portalOffset;
    //    Vector3 portalExitPos = obstaclePos + Vector3.right * portalOffset;

    //    GameObject entranceObj = Instantiate(portalEntrancePrefab, portalEntrancePos, Quaternion.identity);
    //    portalEntranceMangager = entranceObj.GetComponent<PortalEntranceMangager>();
    //    if (portalEntranceMangager != null)
    //    {
    //        portalEntranceMangager.ActionAnimator();
    //    }

    //    GameObject exitObj = Instantiate(portalExitPrefab, portalExitPos, Quaternion.identity);
    //    portalExitManager = exitObj.GetComponent<PortalExitManager>();
    //    if (portalExitManager != null)
    //    {
    //        portalExitManager.ActionAnimator();
    //    }
    //}

    // Code cũ
    //private void CheckObstacleDistance()
    //{
    //    GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

    //    foreach (GameObject obstacle in obstacles)
    //    {
    //        float distance = Vector3.Distance(transform.position, obstacle.transform.position);

    //        if (distance <= portalOffset)
    //        {
    //            CreatePortals(obstacle.transform.position);
    //            Destroy(gameObject);
    //            break;
    //        }
    //    }
    //}

    //private void CreatePortals(Vector3 obstaclePos)
    //{
    //    Vector3 portalEntrancePos = obstaclePos - Vector3.right * portalOffset;
    //    Vector3 portalExitPos = obstaclePos + Vector3.right * portalOffset;

    //    GameObject entranceObj = Instantiate(portalEntrancePrefab, portalEntrancePos, Quaternion.identity);
    //    portalEntranceMangager = entranceObj.GetComponent<PortalEntranceMangager>();
    //    if (portalEntranceMangager != null)
    //    {
    //        portalEntranceMangager.ActionAnimator();
    //    }

    //    GameObject exitObj = Instantiate(portalExitPrefab, portalExitPos, Quaternion.identity);
    //    portalExitManager = exitObj.GetComponent<PortalExitManager>();
    //    if (portalExitManager != null)
    //    {
    //        portalExitManager.ActionAnimator();
    //    }
    //}
}
