using System;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; // Gán qua Inspector
    [SerializeField] private GameObject obstaclePrefab; // Prefab của chướng ngại vật, để trả về pool
    private Camera mainCamera;
    private float cameraLeftEdge;

    private void Awake()
    {
        mainCamera = Camera.main;
        UpdateCameraBrounds();

        //Kiểm tra GameManager
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in the inspector.");
            gameObject.SetActive(false); // Deactivate this object if GameManager is not assigned
        }
    }

    //private void Start()
    //{
    //    gameManager = FindAnyObjectByType<GameManager>();

    //    if (gameManager == null)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Update()
    {
        if (gameManager == null || gameManager.globalSpeed <= 0) return;

        // Di chuyển chướng ngại vật sang trái
        transform.position += Vector3.left * gameManager.globalSpeed * Time.deltaTime;

        UpdateCameraBrounds();

        //Nếu ra khỏi vùng nhìn thấy, trả về pool
        if (transform.position.x < cameraLeftEdge - 2f)
        {
            ObjectPool.Instance.ReturnObject(gameObject, obstaclePrefab);
        }

        //if (transform.position.x < -10f)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void UpdateCameraBrounds()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        cameraLeftEdge = mainCamera.transform.position.x - cameraWidth / 2;
    }
}