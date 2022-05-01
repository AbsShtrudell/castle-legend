using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        triangles.Add(vertices.Count - 1);
    }

    public void AddVertices(Vector3[] vertices)
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                this.vertices.Add(vertices[i]);
                this.triangles.Add(this.vertices.Count - 1);
            }
        }
    }

    public Mesh BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    public void ClearData()
    {
        vertices.Clear();
        triangles.Clear();
    }
}
