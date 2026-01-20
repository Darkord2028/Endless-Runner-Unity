using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject objectToPool;
    private int poolCount;

    private GameObject poolHolder;

    private Queue<GameObject> currentPool = new Queue<GameObject>();

    public void InitializePool(GameObject PoolObject, int PoolCount)
    {
        objectToPool = PoolObject;
        poolCount = PoolCount;

        poolHolder = new GameObject("Pool Holder");

        for (int i = 0; i < poolCount; i++)
        {
            GameObject GO = CreateGameObject();
            GO.SetActive(false);
            GO.transform.parent = poolHolder.transform;
            GO.transform.position = Vector3.zero;
            GO.transform.rotation = Quaternion.identity;
            currentPool.Enqueue(GO);
        }
    }

    private GameObject CreateGameObject()
    {
        return GameObject.Instantiate(objectToPool);
    }

    public GameObject Get()
    {
        if (currentPool.Count == 0)
        {
            return null;
        }

        GameObject GO = currentPool.Dequeue();
        GO.SetActive(true);
        return GO;
    }

    public void ReturnToPool(GameObject GO)
    {
        GO.SetActive(false);
        currentPool.Enqueue(GO);
    }

}
