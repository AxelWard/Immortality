using System.Collections.Generic;
using UnityEngine;

public class ThreadMesh
{
    public string name;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Color> colors;

    public ThreadMesh()
    {
        name = "";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }
}
