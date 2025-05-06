using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class PoolObject
    {
        public GameObject prefab;
        public int initialSize;
    }

    [SerializeField] private List<PoolObject> poolObjects = new List<PoolObject>();

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var poolObject in poolObjects)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolObject.initialSize; i++)
            {
                GameObject obj = Instantiate(poolObject.prefab);
                obj.SetActive(false);

                PooledObject pooledObject = obj.AddComponent<PooledObject>();
                pooledObject.prefabOrigin = poolObject.prefab;

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(poolObject.prefab, objectPool);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("No pool for this prefab: " + prefab.name);
            return null;
        }

        Queue<GameObject> pool = poolDictionary[prefab];

        GameObject obj;

        if (pool.Count == 0)
        {
            obj = Instantiate(prefab);
            obj.SetActive(false);

            PooledObject pooledObject = obj.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                pooledObject = obj.AddComponent<PooledObject>();
            }
            pooledObject.prefabOrigin = prefab;

            pool.Enqueue(obj); // Sau này tái sử dụng
        }

        obj = pool.Dequeue();
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Destroy(obj); // Nếu ko có pool thì destroy luôn
            return;
        }

        obj.SetActive(false);
        poolDictionary[prefab].Enqueue(obj);
    }

    // ✅ Hàm tiện lợi: chỉ cần truyền object, nó sẽ tự tìm prefab gốc để return
    public void ReturnObject(GameObject obj)
    {
        PooledObject pooled = obj.GetComponent<PooledObject>();

        if (pooled != null && pooled.prefabOrigin != null)
        {
            ReturnObject(pooled.prefabOrigin, obj);
        }
        else
        {
            Destroy(obj); // fallback nếu không có prefab gốc
        }
    }
}