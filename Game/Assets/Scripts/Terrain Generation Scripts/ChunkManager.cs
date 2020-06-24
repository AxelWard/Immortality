using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static int CHUNK_SIZE = 25;
    public static int CHUNK_HEIGHT = 50;

    public int chunksAroundPlayer;
    public GameObject chunkPrefab;
    public Transform playerTransform;
    public Vector3 previousChunkLocation;

    private ChunkGenerationJobManager generationJobManager;
    private List<TerrainChunk> loadedChunkList;
    private LoadedChunkMap chunkMap;

    private void Start()
    {
        loadedChunkList = new List<TerrainChunk>();
        chunkMap = new LoadedChunkMap();
        previousChunkLocation = new Vector3(100, 100, 100);
        generationJobManager = gameObject.AddComponent<ChunkGenerationJobManager>();
    }

    private void Update()
    {
        GenerateChunksAroundPlayer();
        CheckForCompletedChunk();
    }

    void GenerateChunksAroundPlayer()
    {
        Vector3 playerChunkLocation = GetPlayerChunkLocation();
        if(playerChunkLocation != previousChunkLocation)
        {
            LoadNewChunksNearPlayer(playerChunkLocation);
            ClearChunksNotNearPlayer(playerChunkLocation);

            previousChunkLocation = playerChunkLocation;
        }
    }

    private void CheckForCompletedChunk()
    {
        if(generationJobManager.hasCompletedJobWaiting)
        {
            LoadChunk(generationJobManager.GetCompletedJob());
        }
    }

    void RemoveChunkJob(Vector3 targetRemovalPositon)
    {
        GenerateChunkJob removalJob = new GenerateChunkJob();
        removalJob.chunkLocationToGenerate = targetRemovalPositon;

        generationJobManager.RemoveGenerationJob(removalJob);
    }

    private void LoadNewChunksNearPlayer(Vector3 playerChunkLocation)
    {
        for(int i = -chunksAroundPlayer; i < chunksAroundPlayer + 1; i++)
        {
            for (int k = -chunksAroundPlayer; k < chunksAroundPlayer + 1; k++)
            {
                if (!chunkMap.IsChunkLoaded(new Vector3(playerChunkLocation.x + i, 0, playerChunkLocation.z + k)))
                {
                    GenerateChunkJob chunkToGenerate = new GenerateChunkJob();
                    chunkToGenerate.chunkLocationToGenerate = new Vector3(playerChunkLocation.x + i, 0, playerChunkLocation.z + k);
                    generationJobManager.AddGenerationJob(chunkToGenerate);
                    chunkMap.AddLoadedChunk(new Vector3(playerChunkLocation.x + i, 0, playerChunkLocation.z + k));
                }
            }
        }
    }

    private void ClearChunksNotNearPlayer(Vector3 playerChunkLocation)
    {
        foreach (TerrainChunk chunk in loadedChunkList.ToArray())
        {
            if (chunk.chunkCoordinates.x < playerChunkLocation.x - chunksAroundPlayer)
            {
                UnloadChunk(chunk);
            }
            else if (chunk.chunkCoordinates.x > playerChunkLocation.x + chunksAroundPlayer)
            {
                UnloadChunk(chunk);
            }
            else if (chunk.chunkCoordinates.z < playerChunkLocation.z - chunksAroundPlayer)
            {
                UnloadChunk(chunk);
            }
            else if (chunk.chunkCoordinates.z > playerChunkLocation.z + chunksAroundPlayer)
            {
                UnloadChunk(chunk);
            }
        }

        for (int i = (int)playerChunkLocation.x - chunksAroundPlayer - 1; i < (int)playerChunkLocation.z + chunksAroundPlayer + 2; i++)
        {
            for (int j = (int)playerChunkLocation.z - chunksAroundPlayer - 1; j < (int)playerChunkLocation.z + chunksAroundPlayer + 2; j++)
            {
                if (!(i < playerChunkLocation.x - chunksAroundPlayer))
                {
                    RemoveChunkJob(new Vector3(i, 0, j));
                }
                else if (!(i > playerChunkLocation.x + chunksAroundPlayer))
                {
                    RemoveChunkJob(new Vector3(i, 0, j));
                }
                else if (!(j < playerChunkLocation.z - chunksAroundPlayer))
                {
                    RemoveChunkJob(new Vector3(i, 0, j));
                }
                else if (!(j > playerChunkLocation.z + chunksAroundPlayer))
                {
                    RemoveChunkJob(new Vector3(i, 0, j));
                }
            }
        }
    }

    void UnloadChunk(TerrainChunk chunk)
    {
        loadedChunkList.Remove(chunk);
        chunkMap.RemoveChunk(chunk.chunkCoordinates);
        chunk.Unload();
    }

    void LoadChunk(GenerateMeshFromChunkJob generatedChunk)
    {
        GameObject current = Instantiate(chunkPrefab, new Vector3(0, 0, 0), new Quaternion());
        current.GetComponent<TerrainChunk>().Load(generatedChunk);
        loadedChunkList.Add(current.GetComponent<TerrainChunk>());
    }

    Vector3 GetPlayerChunkLocation()
    {
        Vector3 playerChunkLocation = new Vector2();

        Vector3 playerPosition = playerTransform.position;
        playerChunkLocation.x = Mathf.FloorToInt(playerPosition.x / (CHUNK_SIZE));
        playerChunkLocation.y = 0;
        playerChunkLocation.z = Mathf.FloorToInt(playerPosition.z / (CHUNK_SIZE));

        return playerChunkLocation;
    }
}
