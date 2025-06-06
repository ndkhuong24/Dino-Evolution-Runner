﻿using System.Collections.Generic;
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

        List<GameObject> spawnedObstacles = new List<GameObject>();

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            GameObject prefab = obstacles[randomIndex];

            if (prefab == null) continue;

            Vector3 spawnOffset = new Vector3(i * 1.5f, 0, 0);
            Vector3 spawnPosition = spawnPoint.position + spawnOffset;

            GameObject spawnedObstacle = ObjectPool.Instance.GetObject(prefab);
            if (spawnedObstacle != null)
            {
                spawnedObstacle.transform.position = spawnPosition;
                spawnedObstacle.transform.rotation = Quaternion.identity;
                spawnedObstacle.transform.parent = null;

                spawnedObstacles.Add(spawnedObstacle);
            }

            StealthSkillBehavior stealth = spawnedObstacle.GetComponent<StealthSkillBehavior>();
            if (stealth != null)
            {
                stealth.ResetToDefault();
            }
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