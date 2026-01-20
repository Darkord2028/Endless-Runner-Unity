using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 chunkSpawnDistance;
    [SerializeField] private int activeChunkCount = 3;
    [SerializeField] private float spawnAheadDistance = 30.0f;

    private ObjectPool chunkPool;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    
    private int chunkCount = 0;

    void Start()
    {
        chunkPool = ChunkPoolManager.instance.ChunkPool;

        for (int i = 0; i < activeChunkCount; i++)
        {
            SpawnChunk();
        }
        
    }

    void Update()
    {
        if (player.position.z + spawnAheadDistance > chunkSpawnDistance.z * chunkCount)
        {
            SpawnChunk();
            RemoveChunk();
        }
    }

    private void SpawnChunk()
    {
        GameObject GO = chunkPool.Get();
        if (GO != null)
        {
            GO.transform.position = chunkSpawnDistance * chunkCount;
        }
        else
        {
            Debug.Log("Cannot get gameobject from chunkpool");
        }

        Chunk chunk = GO.GetComponent<Chunk>();
        if (!chunk)
        {
            Debug.Log("Chunk Component is missing!");
        }
        
        chunk.ActivateChunk();
        activeChunks.Enqueue(chunk);
        chunkCount++;
    }

    private void RemoveChunk()
    {
        if (activeChunks.Count <= activeChunkCount) return;

        Chunk chunk = activeChunks.Dequeue();
        chunk.DeactivateChunk();
        chunkPool.ReturnToPool(chunk.gameObject);
    }

}
