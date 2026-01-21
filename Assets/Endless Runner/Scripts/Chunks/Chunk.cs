using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] Transform[] obstaclePoints;
    [SerializeField] float coinSpawnHeight = 5.0f;

    private List<GameObject> activeCoins = new List<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    public void ActivateChunk()
    {
        foreach (Transform obstaclePoint in obstaclePoints)
        {
            GameObject obstacleGO = ObstaclePoolManager.instance.GetRandomObstacle();

            if (!obstacleGO)
            {
                return;
            }

            obstacleGO.transform.position = obstaclePoint.position;
            obstacleGO.transform.rotation = obstaclePoint.rotation;
            activeObstacles.Add(obstacleGO);
        }

        SpawnCoins();

    }

    public void DeactivateChunk()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            ObstaclePoolManager.instance.ReturnObstacleToPool(obstacle);
        }
        foreach (GameObject coin in activeCoins)
        {
            CoinPoolManager.instance.CoinPool.ReturnToPool(coin);
        }
        activeObstacles.Clear();
        activeCoins.Clear();
    }

    private void SpawnCoins()
    {
        foreach (GameObject GO in activeObstacles)
        {
            Obstacles obstacle = GO.GetComponent<Obstacles>();
            if (!obstacle || obstacle.coinPattern == CoinPatterns.NONE)
            {
                continue;
            }

            List<Vector3> coinPositions = CoinPatternsLibrary.GetPattern(obstacle.coinPattern, GO.transform, 1, 10);
            
            foreach (Vector3 pos in coinPositions)
            {
                GameObject coinGO = CoinPoolManager.instance.CoinPool.Get();
                if (!coinGO)
                {
                    return;
                }
                coinGO.transform.position = pos + Vector3.up * coinSpawnHeight;
                coinGO.transform.rotation = Quaternion.identity;
                activeCoins.Add(coinGO);
            }

        }
    }

}
