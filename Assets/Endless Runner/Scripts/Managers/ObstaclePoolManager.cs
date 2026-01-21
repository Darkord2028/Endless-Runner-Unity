using UnityEngine;

public class ObstaclePoolManager : MonoBehaviour
{
    public static ObstaclePoolManager instance;

    [SerializeField] GameObject[] obstaclesPrefab;
    [SerializeField] int poolSizePerObstacle = 10;

    private ObjectPool[] obstaclesPool;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        obstaclesPool = new ObjectPool[obstaclesPrefab.Length];
        for (int i = 0; i < obstaclesPrefab.Length; i++)
        {
            obstaclesPool[i] = new ObjectPool();
            obstaclesPool[i].InitializePool(obstaclesPrefab[i], poolSizePerObstacle);
        }
    }

    public GameObject GetRandomObstacle()
    {
        int index = Random.Range(0, obstaclesPool.Length);
        return obstaclesPool[index].Get();
    }

    public void ReturnObstacleToPool(GameObject obstacle)
    {
        PooledObject pooledObject = obstacle.GetComponent<PooledObject>();
        if (pooledObject != null && pooledObject.originPool != null)
        {
            pooledObject.originPool.ReturnToPool(obstacle);
        }
        else
        {
            Debug.LogWarning($"{obstacle.name} does not have a PooledObject component!");
        }
    }

}
