using TreeEditor;
using UnityEngine;

public class PerlinCalculator
{
    public static float perlinScale = .25f;
    public static float heightLimit = 50;
    public static float layerScaleReduction = .5f;
    public static float layerStrengthReduction = .1f;
    public static float voxelThreshold = .5f;

    public static int GetHeightAtCoords(int x, int z, int numberOfOctaves)
    {
        float height = 0f;
        float layerStrength = 1f;

        for(int i = 0; i < numberOfOctaves; i++)
        {
            height += Get2DPerlinValue(x, z, i + 1) * layerStrength;
            layerStrength *= layerStrengthReduction;
        }

        return Mathf.RoundToInt(height);
    }

    static float Get2DPerlinValue(int xCoord, int zCoord, int octave)
    {
        int chunkSize = ChunkManager.CHUNK_SIZE;
        float octaveSize = perlinScale;

        float x = ((float)xCoord / chunkSize) * (octaveSize);
        float z = ((float)zCoord / chunkSize) * (octaveSize);

        return (Mathf.PerlinNoise(x, z) * heightLimit);
    }

    public static bool GetVoxelAtLocation(int xCoord, int yCoord, int zCoord, int numberOfOctaves)
    {
        bool voxelOn = false;
        int chunkSize = ChunkManager.CHUNK_SIZE;
        float perlinValue = 0f;

        for (int i = 1; i < numberOfOctaves + 1; i++)
        {
            float octaveSize = perlinScale * (1f / i);

            float x = ((float)xCoord / chunkSize) * octaveSize * perlinScale;
            float y = ((float)yCoord / chunkSize) * octaveSize * perlinScale;
            float z = ((float)zCoord / chunkSize) * octaveSize * perlinScale;

            perlinValue += Get3DPerlinValue(x, y, z);
        }

        if (perlinValue > voxelThreshold)
        {
            voxelOn = true;
        }

        return voxelOn;
    }

    static float Get3DPerlinValue(float x, float y, float z)
    {
        float perlinValue = 0f;

        perlinValue += Mathf.PerlinNoise(x, y);
        perlinValue += Mathf.PerlinNoise(y, z);
        perlinValue += Mathf.PerlinNoise(x, z);

        perlinValue += Mathf.PerlinNoise(y, x);
        perlinValue += Mathf.PerlinNoise(z, y);
        perlinValue += Mathf.PerlinNoise(z, x);

        perlinValue = perlinValue / 6;

        return perlinValue;
    }
}
