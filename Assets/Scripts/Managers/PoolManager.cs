using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = pools[prefab];
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        PooledObject pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj != null)
        {
            pooledObj.prefabReference = prefab;
            pooledObj.OnSpawned();
        }

        return obj;
    }

    public void Despawn(GameObject obj, GameObject prefab = null)
    {
        PooledObject pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj == null || pooledObj.prefabReference == null)
        {
            Destroy(obj);
            return;
        }

        GameObject key = pooledObj.prefabReference;

        obj.SetActive(false);
        pooledObj.OnDespawned();

        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();

        pools[key].Enqueue(obj);
    }
}