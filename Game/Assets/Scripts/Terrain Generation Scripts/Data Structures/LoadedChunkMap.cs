using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedChunkMap
{
    List<Vector3> loadedChunks;

    public LoadedChunkMap()
    {
        loadedChunks = new List<Vector3>();
    }

    public bool IsChunkLoaded(Vector3 chunkCoordinates)
    {
        foreach(Vector3 check in loadedChunks)
        {
            if(chunkCoordinates == check)
            {
                return true;
            }
        }

        return false;
    }

    public void AddLoadedChunk(Vector3 chunkCoordinates)
    {
        loadedChunks.Add(chunkCoordinates);
    }

    public void RemoveChunk(Vector3 chunkCoordinates)
    {
        loadedChunks.Remove(chunkCoordinates);
    }
}
