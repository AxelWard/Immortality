using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkMeshGenerator
{
    public static float cubeScale = .02f;

    Mesh mesh;

    List<Vector3> meshVertices;
    List<int> meshTriangles;
    List<Color> meshColors;

    Vector3 chunkCoordinates;
    bool[,,] chunkVoxelArray;
    int chunkSize;

    public Mesh GenerateChunkMesh(TerrainChunk chunkToDraw)
    {
        mesh = new Mesh();
        mesh.name = "Terrain Mesh";

        meshVertices = new List<Vector3>();
        meshTriangles = new List<int>();
        meshColors = new List<Color>();

        chunkCoordinates = chunkToDraw.chunkCoordinates;
        chunkVoxelArray = chunkToDraw.chunkVoxelArray;

        BuildMesh();
        UpdateMesh();

        return mesh;
    }

    private void BuildMesh()
    {
        for(int i = 0; i < chunkVoxelArray.GetLength(0); i++)
        {
            for (int j = 0; j < chunkVoxelArray.GetLength(1); j++)
            {
                for (int k = 0; k < chunkVoxelArray.GetLength(2); k++)
                {
                    if(chunkVoxelArray[i,j,k])
                    {
                        GenerateCube(i, j, k);
                    }
                }
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshTriangles.ToArray();
        mesh.colors = meshColors.ToArray();
        mesh.RecalculateNormals();
    }

    private void GenerateCube(int xCoord, int yCoord, int zCoord)
    {
        int start;
        Vector3 offset = chunkCoordinates * ChunkManager.CHUNK_SIZE;

        // Top
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 3);

        // Bottom
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 3);
        meshTriangles.Add(start + 2);

        // Front
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 3);

        // Back
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 3);
        meshTriangles.Add(start + 2);

        // Left
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord + cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 3);

        // Right
        start = GetLargestIndex() + 1;

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord + cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord - cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshVertices.Add(new Vector3(xCoord - cubeScale, yCoord + cubeScale, zCoord - cubeScale) + offset);
        meshColors.Add(new Color(.5f, .5f, .5f));

        meshTriangles.Add(start);
        meshTriangles.Add(start + 2);
        meshTriangles.Add(start + 1);
        meshTriangles.Add(start);
        meshTriangles.Add(start + 3);
        meshTriangles.Add(start + 2);
    }

    private int GetLargestIndex()
    {
        int index = -1;

        foreach(int check in meshTriangles)
        {
            if(check > index)
            {
                index = check;
            }
        }

        return index;
    }
}
