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

    private List<GameObject> activeCoins = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void ActivateChunk()
    {
        List<Vector3> positions = CoinPatterns.Straight(coinPoints[0].position, 10, 0.5f);

        int currentCoinIndex = 0;

        foreach (Vector3 pos in positions)
        {
            GameObject coin = CoinPoolManager.instance.CoinPool.Get();
            if (coin != null)
            {
                coin.transform.position = pos;
                activeCoins.Add(coin);
                currentCoinIndex++;
            }
        }
    }

    public void DeactivateChunk()
    {
        foreach(GameObject coin in activeCoins)
        {
            CoinPoolManager.instance.CoinPool.ReturnToPool(coin);
        }
    }

}
