using UnityEngine;

public class ChunkPoolManager : MonoBehaviour
{
    public static ChunkPoolManager instance;

    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int poolSize = 20;

    public ObjectPool ChunkPool { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        ChunkPool = new ObjectPool();
        ChunkPool.InitializePool(chunkPrefab, poolSize);
    }
}
