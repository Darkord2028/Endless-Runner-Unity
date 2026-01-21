using System.Collections.Generic;
using UnityEngine;

public enum CoinSpawnPattern
{
    STRAIGHT,
    ZIGZAG,
    ALLLANES,
    JUMPARC,
    RANDOMSPARSE,
    WAVE
}

public class Chunk : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] Transform[] coinPoints;
    [SerializeField] Transform[] obstaclePoints;

    //private List<GameObject> activeCoins = new List<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    public void ActivateChunk()
    {
        foreach (Transform obstaclePoint in obstaclePoints)
        {
            GameObject obstacle = ObstaclePoolManager.instance.GetRandomObstacle();

            if (obstacle == null)
            {
                Debug.Log("Cannot get obstacle from pool");
                return;
            }

            obstacle.transform.position = obstaclePoint.position;
            obstacle.transform.rotation = obstaclePoint.rotation;
            activeObstacles.Add(obstacle);
        }
    }

    public void DeactivateChunk()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            ObstaclePoolManager.instance.ReturnObstacleToPool(obstacle);
        }
        activeObstacles.Clear();
    }

}
