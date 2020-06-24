using UnityEngine;

public class GenerateChunkJob : BaseThreadedJob
{
    public string JobID;
    public Vector3 chunkLocationToGenerate;
    public bool[,,] chunkVoxelArray;

    protected override void ThreadFunction()
    {
        int chunkSize = ChunkManager.CHUNK_SIZE;
        int chunkHeight = ChunkManager.CHUNK_HEIGHT;

        Vector3 offset = chunkLocationToGenerate * chunkSize;

        chunkVoxelArray = new bool[chunkSize + 1, chunkHeight, chunkSize + 1];
        for (int i = 0; i < chunkVoxelArray.GetLength(0); i++)
        {
            for (int j = 0; j < chunkVoxelArray.GetLength(2); j++)
            {
                int height = PerlinCalculator.GetHeightAtCoords(i + (int)offset.x, j + (int)offset.z, 2);

                for(int k = 0; k < chunkVoxelArray.GetLength(1); k++)
                {
                    if(k <= height)
                    {
                        chunkVoxelArray[i, k, j] = true;
                    }
                }
            }
        }
    }

    protected override void OnFinished()
    {

    }

    public bool Equals(GenerateChunkJob check)
    {
        if(check == null)
        {
            return false;
        }

        if(chunkLocationToGenerate == check.chunkLocationToGenerate)
        {
            return true;
        }

        return false;
    }

    public GenerateMeshFromChunkJob ToMeshJob()
    {
        GenerateMeshFromChunkJob meshJob = new GenerateMeshFromChunkJob();
        meshJob.chunkCoordinates = chunkLocationToGenerate;
        return meshJob;
    }
}
