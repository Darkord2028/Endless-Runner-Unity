using UnityEngine;

public class CoinPoolManager : MonoBehaviour
{
    public static CoinPoolManager instance;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] int poolSize = 20;

    public ObjectPool CoinPool { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        CoinPool = new ObjectPool();
        CoinPool.InitializePool(coinPrefab, poolSize);
    }
}
