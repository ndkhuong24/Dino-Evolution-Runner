using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public Transform spawnPoint;

    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 2.2f;

    public float spawnAcceleration = 0.93f;  // Tăng tốc spawn nhanh hơn theo thời gian
    public float spawnVariance = 1.2f;       // Ngẫu nhiên hóa thời gian spawn
    public float doubleSpawnChance = 0.4f;   // 40% cơ hội spawn 2 chướng ngại vật cùng lúc
    public float tripleSpawnChance = 0.15f;  // 15% cơ hội spawn 3 chướng ngại vật cùng lúc

    private float nextSpawnTime;
    private float currentMinSpawnTime;
    private float currentMaxSpawnTime;

    void Start()
    {
        currentMinSpawnTime = minSpawnTime;
        currentMaxSpawnTime = maxSpawnTime;
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

        // Xác suất spawn 2 hoặc 3 chướng ngại vật cùng lúc
        if (Random.value < tripleSpawnChance)
        {
            spawnCount = 3; // 15% cơ hội spawn 3 chướng ngại vật
        }
        else if (Random.value < doubleSpawnChance)
        {
            spawnCount = 2; // 40% cơ hội spawn 2 chướng ngại vật
        }

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
        currentMinSpawnTime *= spawnAcceleration;
        currentMaxSpawnTime *= spawnAcceleration;

        currentMinSpawnTime = Mathf.Max(0.4f, currentMinSpawnTime);
        currentMaxSpawnTime = Mathf.Max(0.8f, currentMaxSpawnTime);

        // 20% cơ hội làm thời gian spawn nhanh hơn cực nhanh
        if (Random.value > 0.8f)
        {
            currentMinSpawnTime *= 0.8f;
            currentMaxSpawnTime *= 0.8f;
        }
    }
}
