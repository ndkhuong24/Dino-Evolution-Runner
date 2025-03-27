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

    private void Start()
    {
        currentMinSpawnTime = minSpawnTime;
        currentMaxSpawnTime = maxSpawnTime;
        minSpawnLimit = minSpawnTime * 0.7f;
        maxSpawnLimit = maxSpawnTime * 0.7f;

        SetNextSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacleGroup();
            SetNextSpawnTime();
            ModifySpawnRate();
        }
    }

    private void SpawnObstacleGroup()
    {
        if (obstacles == null || obstacles.Length == 0) return;

        int spawnCount = Random.value < doubleSpawnChance ? 2 : 1;

        GameObject obstacleGroup = null;

        if (spawnCount > 1)
        {
            obstacleGroup = new GameObject("ObstacleGroup"); // Tạo nhóm nếu có hơn 1 obstacle
            obstacleGroup.tag = "ObstacleGroup";
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);

            if (obstacles[randomIndex] == null) continue;

            Vector3 spawnOffset = new Vector3(i * 1.5f, 0, 0);
            GameObject spawnedObstacle = Instantiate(obstacles[randomIndex], spawnPoint.position + spawnOffset, Quaternion.identity);

            if (spawnedObstacle == null) continue;

            if (obstacleGroup != null)
            {
                spawnedObstacle.transform.parent = obstacleGroup.transform; // Chỉ gán parent nếu có group
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
