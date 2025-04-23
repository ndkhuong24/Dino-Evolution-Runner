using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles; // Các prefab chướng ngại vật
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 1.5f;
    [SerializeField] private float maxSpawnTime = 3.0f;
    [SerializeField] private float spawnAcceleration = 0.99f;
    private float minSpawnLimit;
    private float maxSpawnLimit;

    [Header("Randomization")]
    [SerializeField] private float doubleSpawnChance = 0.3f;

    [Header("Level Progression")]
    [SerializeField] private float levelDuration = 30f; // Mỗi level kéo dài 30 giây
    private int currentLevel = 1;
    private float levelTimer = 0f;

    [Header("Spawn Optimization")]
    [SerializeField] private int maxObstaclesInView = 3; // Số lượng tối đa chướng ngại vật trong vùng nhìn thấy

    private float nextSpawnTime;
    private float currentMinSpawnTime;
    private float currentMaxSpawnTime;

    // Vùng nhìn thấy (dựa trên camera)
    private Camera mainCamera;
    private float cameraRightEdge;
    private float cameraLeftEdge;
    private List<GameObject> activeGroups = new List<GameObject>(); // Lưu các nhóm chướng ngại vật

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateCameraBounds();

        currentMinSpawnTime = minSpawnTime;
        currentMaxSpawnTime = maxSpawnTime;
        minSpawnLimit = minSpawnTime * 0.7f;
        maxSpawnLimit = maxSpawnTime * 0.7f;

        SetNextSpawnTime();
    }

    private void Update()
    {
        UpdateCameraBounds();
        UpdateLevel();

        // Kiểm tra số lượng chướng ngại vật trong vùng nhìn thấy trước khi spawn
        if (Time.time >= nextSpawnTime && CanSpawn())
        {
            SpawnObstacleGroup();
            SetNextSpawnTime();
            ModifySpawnRate();
        }

        CheckAndReturnGroups();
    }

    private void UpdateCameraBounds()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        cameraRightEdge = mainCamera.transform.position.x + cameraWidth / 2;
        cameraLeftEdge = mainCamera.transform.position.x - cameraWidth / 2;
    }

    private void UpdateLevel()
    {
        levelTimer += Time.deltaTime;
        if (levelTimer >= levelDuration)
        {
            currentLevel = Mathf.Min(currentLevel + 1, 3); // Tối đa 3 level
            levelTimer = 0f;

            // Điều chỉnh độ khó theo level
            if (currentLevel == 1)
            {
                Time.timeScale = 1f;
            }
            else if (currentLevel == 2)
            {
                Time.timeScale = 1.5f;
            }
            else if (currentLevel == 3)
            {
                Time.timeScale = 2f;
            }
        }
    }

    private bool CanSpawn()
    {
        // Đếm số lượng chướng ngại vật đang trong vùng nhìn thấy
        int obstaclesInView = 0;

        foreach (var group in activeGroups)
        {
            if (group == null) continue;

            float groupXPosition = group.transform.position.x;
            if (group.tag == "ObstacleGroup")
            {
                // Với nhóm, lấy vị trí của chướng ngại vật cuối cùng trong nhóm
                Transform lastChild = group.transform.GetChild(group.transform.childCount - 1);
                groupXPosition = lastChild.position.x;
            }

            // Nếu nhóm (hoặc chướng ngại vật đơn lẻ) nằm trong vùng nhìn thấy
            if (groupXPosition >= cameraLeftEdge && groupXPosition <= cameraRightEdge)
            {
                obstaclesInView += (group.tag == "ObstacleGroup") ? group.transform.childCount : 1;
            }
        }

        // Chỉ spawn nếu số lượng chướng ngại vật trong vùng nhìn thấy nhỏ hơn ngưỡng tối đa
        return obstaclesInView < maxObstaclesInView;
    }

    private void SpawnObstacleGroup()
    {
        if (obstacles == null || obstacles.Length == 0) return;

        int spawnCount = Random.value < doubleSpawnChance ? 2 : 1;
        List<GameObject> spawnedObstacles = new List<GameObject>();

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            if (obstacles[randomIndex] == null) continue;

            // Lấy chướng ngại vật từ ObjectPool
            GameObject spawnedObstacle = ObjectPool.Instance.GetObject(obstacles[randomIndex]);
            if (spawnedObstacle == null) continue;

            Vector3 spawnOffset = new Vector3(i * 1.5f, 0, 0);
            spawnedObstacle.transform.position = spawnPoint.position + spawnOffset;
            spawnedObstacle.transform.rotation = Quaternion.identity;

            spawnedObstacles.Add(spawnedObstacle);
        }

        // Nếu có nhiều hơn 1 thì gộp vào nhóm
        if (spawnedObstacles.Count > 1)
        {
            GameObject obstacleGroup = new GameObject("ObstacleGroup");
            obstacleGroup.tag = "ObstacleGroup";

            foreach (var obs in spawnedObstacles)
            {
                obs.transform.parent = obstacleGroup.transform;
            }

            activeGroups.Add(obstacleGroup);
        }
        else if (spawnedObstacles.Count == 1)
        {
            activeGroups.Add(spawnedObstacles[0]); // Nếu chỉ có 1, không cần nhóm
        }
    }

    private void CheckAndReturnGroups()
    {
        for (int i = activeGroups.Count - 1; i >= 0; i--)
        {
            GameObject group = activeGroups[i];
            if (group == null) continue;

            // Kiểm tra vị trí của nhóm (hoặc chướng ngại vật đơn lẻ)
            float groupXPosition = group.transform.position.x;
            if (group.tag == "ObstacleGroup")
            {
                // Với nhóm, lấy vị trí của chướng ngại vật cuối cùng trong nhóm
                Transform lastChild = group.transform.GetChild(group.transform.childCount - 1);
                groupXPosition = lastChild.position.x;
            }

            // Nếu ra khỏi vùng nhìn thấy, trả về pool
            if (groupXPosition < cameraLeftEdge - 2f)
            {
                if (group.tag == "ObstacleGroup")
                {
                    // Trả từng chướng ngại vật trong nhóm về pool
                    foreach (Transform child in group.transform)
                    {
                        foreach (var prefab in obstacles)
                        {
                            if (child.gameObject.name.Contains(prefab.name))
                            {
                                ObjectPool.Instance.ReturnObject(child.gameObject, prefab);
                                break;
                            }
                        }
                    }
                    Destroy(group); // Phá hủy nhóm, nhưng không phá hủy chướng ngại vật
                }
                else
                {
                    // Nếu là chướng ngại vật đơn lẻ
                    foreach (var prefab in obstacles)
                    {
                        if (group.name.Contains(prefab.name))
                        {
                            ObjectPool.Instance.ReturnObject(group, prefab);
                            break;
                        }
                    }
                }
                activeGroups.RemoveAt(i);
            }
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(currentMinSpawnTime, currentMaxSpawnTime);
    }

    private void ModifySpawnRate()
    {
        currentMinSpawnTime = Mathf.Max(minSpawnLimit, currentMinSpawnTime * spawnAcceleration);
        currentMaxSpawnTime = Mathf.Max(maxSpawnLimit, currentMaxSpawnTime * spawnAcceleration);
    }
}