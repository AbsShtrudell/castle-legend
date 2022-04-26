using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();

    public void AddVertex(Vector3 vertex)
    {
        _vertices.Add(vertex);
        _triangles.Add(_vertices.Count - 1);
    }

    public void AddVertices(Vector3[] vertices)
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                _vertices.Add(vertices[i]);
                _triangles.Add(_vertices.Count - 1);
            }
        }
    }

    public Mesh BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    public void ClearData()
    {
        _vertices.Clear();
        _triangles.Clear();
    }
}
