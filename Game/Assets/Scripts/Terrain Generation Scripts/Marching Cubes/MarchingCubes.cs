using UnityEngine;

public class MarchingCubes
{
    public bool[,,] voxelsToMarch;
    public Vector3 meshOffset;

    private ThreadMesh marchedMesh;
    private Vector3 currentMarchCoordinates;
    
    public ThreadMesh March()
    {
        marchedMesh = new ThreadMesh();
        for (int j = 0; j < voxelsToMarch.GetLength(1) - 1; j++)
        {
            for (int i = 0; i < voxelsToMarch.GetLength(0) - 1; i++)
            {
                for (int k = 0; k < voxelsToMarch.GetLength(2) - 1; k++)
                {
                    generateMeshAt(i, j, k);
                }
            }
        }

        return marchedMesh;
    }

    private void generateMeshAt(int xCoord, int yCoord, int zCoord)
    {
        int lookupValue = GetValueFromCoords(xCoord, yCoord, zCoord);

        currentMarchCoordinates = new Vector3(xCoord, yCoord, zCoord);

        int[] triangles = TriangulationTable.lookupTable[lookupValue];
        AddVertices(triangles);
    }

    private int GetValueFromCoords(int xCoord, int yCoord, int zCoord)
    {
        int lookupValue = 0;

        // V1
        if(voxelsToMarch[xCoord, yCoord, zCoord]) { lookupValue += 1; }
        // V2
        if (voxelsToMarch[xCoord + 1, yCoord, zCoord]) { lookupValue += 2; }
        // V3
        if (voxelsToMarch[xCoord + 1, yCoord + 1, zCoord]) { lookupValue += 4; }
        // V4
        if (voxelsToMarch[xCoord, yCoord + 1, zCoord]) { lookupValue += 8; }
        // V5
        if (voxelsToMarch[xCoord, yCoord, zCoord + 1]) { lookupValue += 16; }
        // V6
        if (voxelsToMarch[xCoord + 1, yCoord, zCoord + 1]) { lookupValue += 32; }
        // V7
        if (voxelsToMarch[xCoord + 1, yCoord + 1, zCoord + 1]) { lookupValue += 64; }
        // V8
        if (voxelsToMarch[xCoord, yCoord + 1, zCoord + 1]) { lookupValue += 128; }

        return lookupValue;
    }

    public void AddVertices(int[] vertices)
    {
        for(int i = 0; i < vertices.Length; i += 3)
        {
            marchedMesh.vertices.Add(EdgePointLocationTable.edgePoints[vertices[i + 2]] + meshOffset * ChunkManager.CHUNK_SIZE + currentMarchCoordinates);
            marchedMesh.triangles.Add(marchedMesh.vertices.Count - 1);
            marchedMesh.colors.Add(new Color(0.5f, 0.5f, 0.5f));

            marchedMesh.vertices.Add(EdgePointLocationTable.edgePoints[vertices[i + 1]] + meshOffset * ChunkManager.CHUNK_SIZE + currentMarchCoordinates);
            marchedMesh.triangles.Add(marchedMesh.vertices.Count - 1);
            marchedMesh.colors.Add(new Color(0.5f, 0.5f, 0.5f));

            marchedMesh.vertices.Add(EdgePointLocationTable.edgePoints[vertices[i]] + meshOffset * ChunkManager.CHUNK_SIZE + currentMarchCoordinates);
            marchedMesh.triangles.Add(marchedMesh.vertices.Count - 1);
            marchedMesh.colors.Add(new Color(0.5f, 0.5f, 0.5f));
        }
    }
}
