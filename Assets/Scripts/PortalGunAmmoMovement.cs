using System;
using UnityEngine;

public class PortalGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f;
    private Animator animator;
    private float destroyBoundaryX;

    [Header("Portal Settings")]
    public GameObject portalEntrancePrefab;
    public GameObject portalExitPrefab;
    public float portalOffset = 1.2f;

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

        if (transform.position.x > destroyBoundaryX) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject targetObject = collision.transform.parent?.CompareTag("ObstacleGroup") ?? false
            ? collision.transform.parent.gameObject
            : collision.gameObject;

        if (targetObject.CompareTag("Obstacle") || targetObject.CompareTag("ObstacleGroup"))
        {
            CreatePortals(targetObject);
            Destroy(gameObject);
        }
    }

    private void CreatePortals(GameObject obstacle)
    {
        (float minX, float maxX, float referenceY) = GetObstacleBounds(obstacle);

        Vector3 portalEntrancePos = new Vector3(minX - portalOffset, referenceY, 0);
        Vector3 portalExitPos = new Vector3(maxX + portalOffset, referenceY, 0);

        GameObject portalEntrance = Instantiate(portalEntrancePrefab, portalEntrancePos, Quaternion.identity);
        GameObject portalExit = Instantiate(portalExitPrefab, portalExitPos, Quaternion.identity);

        // Liên kết portal entrance với portal exit
        PortalEntranceMangager entranceManager = portalEntrance.GetComponent<PortalEntranceMangager>();
        PortalExitManager exitManager = portalExit.GetComponent<PortalExitManager>();

        entranceManager.ActionAnimator();
        exitManager.ActionAnimator();

        if (entranceManager != null && exitManager != null)
        {
            entranceManager.linkedPortalExit = exitManager.transform;
        }
    }

    private (float, float, float) GetObstacleBounds(GameObject obstacle)
    {
        Collider2D[] colliders = obstacle.GetComponentsInChildren<Collider2D>();

        if (colliders.Length == 0)
        {
            float posX = obstacle.transform.position.x;
            float posY = obstacle.transform.position.y;
            return (posX, posX, posY);
        }

        float minX = Mathf.Infinity, maxX = Mathf.NegativeInfinity;
        float referenceY = colliders[0].bounds.center.y;

        foreach (var col in colliders)
        {
            minX = Mathf.Min(minX, col.bounds.min.x);
            maxX = Mathf.Max(maxX, col.bounds.max.x);
        }

        return (minX, maxX, referenceY);
    }
}