using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainChunk : MonoBehaviour
{
    public string TerrainID;

    public Vector3 chunkCoordinates;
    public Gradient colors;
    public bool[,,] chunkVoxelArray;

    public void Unload()
    {
        Destroy(gameObject);
    }

    public void Load(GenerateMeshFromChunkJob generatedChunkJob)
    {
        chunkVoxelArray = generatedChunkJob.chunkVoxelArray;
        chunkCoordinates = generatedChunkJob.chunkCoordinates;
        UpdateMesh(generatedChunkJob.generatedMesh);
    }

    private void UpdateMesh(ThreadMesh m)
    {
        Mesh generatedMesh = new Mesh();

        generatedMesh.name = m.name;
        generatedMesh.vertices = m.vertices.ToArray();
        generatedMesh.triangles = m.triangles.ToArray();
        generatedMesh.colors = m.colors.ToArray();
        generatedMesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = generatedMesh;
        GetComponent<MeshCollider>().sharedMesh = generatedMesh;
    }
}
