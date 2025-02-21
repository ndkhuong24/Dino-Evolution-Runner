using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public Transform spawnPoint;

    [Header("Spawn Timing")]
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3.0f;
    public float spawnAcceleration = 0.99f;
    private float minSpawnLimit;
    private float maxSpawnLimit;

    [Header("Randomization")]
    public float doubleSpawnChance = 0.3f;

    private float nextSpawnTime;
    private float currentMinSpawnTime;
    private float currentMaxSpawnTime;
    //private float lastSpawnX = -10f;

    void Start()
    {
        currentMinSpawnTime = minSpawnTime;
        currentMaxSpawnTime = maxSpawnTime;

        minSpawnLimit = minSpawnTime * 0.7f;
        maxSpawnLimit = maxSpawnTime * 0.7f;

        SetNextSpawnTime();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            SetNextSpawnTime();
            ModifySpawnRate();
        }
    }

    void SpawnObstacle()
    {
        int spawnCount = 1;

        if (Random.value < doubleSpawnChance) spawnCount = 2; // Chỉ còn spawn 1 hoặc 2 chướng ngại vật

        //for (int i = 0; i < spawnCount; i++)
        //{
        //    if (spawnPoint.position.x - lastSpawnX < 2.0f) return; // Ngăn chướng ngại vật bị chồng lên nhau

        //    int randomIndex = Random.Range(0, obstacles.Length);
        //    Vector3 spawnOffset = new Vector3(i * 1.5f, 0, 0);
        //    Instantiate(obstacles[randomIndex], spawnPoint.position + spawnOffset, Quaternion.identity);
        //    lastSpawnX = spawnPoint.position.x;
        //}
        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            Vector3 spawnOffset = new Vector3(i * 1.5f, 0, 0); // Để chúng không chồng lên nhau
            GameObject obstacle = Instantiate(obstacles[randomIndex], spawnPoint.position + spawnOffset, Quaternion.identity);
            obstacle.tag = "Obstacle";
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(currentMinSpawnTime, currentMaxSpawnTime);
    }

    void ModifySpawnRate()
    {
        currentMinSpawnTime = Mathf.Max(minSpawnLimit, currentMinSpawnTime * spawnAcceleration);
        currentMaxSpawnTime = Mathf.Max(maxSpawnLimit, currentMaxSpawnTime * spawnAcceleration);

        // 🎯 Nếu tốc độ của obstacles đạt giới hạn thì nền đất cũng không tăng tốc nữa
        if (currentMinSpawnTime == minSpawnLimit)
        {
            FindFirstObjectByType<GameManager>().speedIncreaseRate = 0f;
        }
    }
}
