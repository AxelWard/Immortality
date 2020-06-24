using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshFromChunkJob : BaseThreadedJob
{
    public static float cubeScale = .25f;

    public string JobID;
    public Vector3 chunkCoordinates;
    public bool[,,] chunkVoxelArray;

    public ThreadMesh generatedMesh;

    protected override void ThreadFunction()
    {
        generatedMesh = new ThreadMesh();
        generatedMesh.name = "Terrain Mesh";

        BuildMesh();
    }

    protected override void OnFinished()
    {
        
    }

    private void BuildMesh()
    {
        MarchingCubes marcher = new MarchingCubes();
        marcher.voxelsToMarch = chunkVoxelArray;
        marcher.meshOffset = chunkCoordinates;

        generatedMesh = marcher.March();
    }

    public bool Equals(GenerateChunkJob check)
    {
        if (check == null)
        {
            return false;
        }

        if (check.chunkLocationToGenerate == chunkCoordinates)
        {
            return true;
        }

        return false;
    }

    public bool Equals(GenerateMeshFromChunkJob check)
    {
        if (check == null)
        {
            return false;
        }

        if (check.chunkCoordinates == chunkCoordinates)
        {
            return true;
        }

        return false;
    }
}
