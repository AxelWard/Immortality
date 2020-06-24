using UnityEngine;

public class ChunkVoxelArrayCreator
{
    public static bool[,,] CreateChunkVoxelArray(Vector3 chunkCoordinates)
    {
        int chunkSize = ChunkManager.CHUNK_SIZE;
        int chunkHeight = ChunkManager.CHUNK_HEIGHT;

        Vector3 offset = chunkCoordinates * chunkSize;

        bool[,,] chunkArray = new bool[chunkSize, chunkHeight, chunkSize];
        for (int i = 0; i < chunkArray.GetLength(0); i++)
        {
            for (int j = 0; j < chunkArray.GetLength(2); j++)
            {
                int height = PerlinCalculator.GetHeightAtCoords(i + (int)offset.x, j + (int)offset.z, 2);

                chunkArray[i, height, j] = true;
            }
        }

        return chunkArray;
    }
}
