using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    // Singleton instance
    public static ObjectPool Instance { get; private set; }

    // Dictionary để lưu các pool theo loại đối tượng (key là prefab, value là queue chứa các đối tượng)
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    // Danh sách để thiết lập các pool trong Unity Editor
    [SerializeField] private List<PoolObject> poolObjects = new List<PoolObject>();

    [System.Serializable]
    public class PoolObject
    {
        public GameObject prefab; // Prefab của đối tượng (obstacle, power-up...)
        public int initialSize;   // Số lượng ban đầu trong pool
    }

    private void Awake()
    {
        // Khởi tạo singleton
        Instance = this;

        // Khởi tạo pool cho từng loại đối tượng
        foreach (var poolObject in poolObjects)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Tạo số lượng đối tượng ban đầu
            for (int i = 0; i < poolObject.initialSize; i++)
            {
                GameObject obj = Instantiate(poolObject.prefab);
                obj.SetActive(false); // Tắt đối tượng ban đầu
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(poolObject.prefab, objectPool);
        }
    }

    // Lấy một đối tượng từ pool
    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Prefab {prefab.name} không có trong Object Pool!");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[prefab];
        GameObject obj;

        // Nếu pool trống, tạo thêm đối tượng mới
        if (pool.Count == 0)
        {
            obj = Instantiate(prefab);
        }
        else
        {
            obj = pool.Dequeue();
        }

        obj.SetActive(true);
        return obj;
    }

    // Trả đối tượng về pool
    public void ReturnObject(GameObject obj, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Prefab {prefab.name} không có trong Object Pool!");
            return;
        }

        obj.SetActive(false);
        poolDictionary[prefab].Enqueue(obj);
    }
}