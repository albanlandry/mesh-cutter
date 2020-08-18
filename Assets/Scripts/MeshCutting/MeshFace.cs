using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFace {
    private Vector3[] vertices;
    private List<int[]> face; // Hold the triangle that form the face

    public MeshFace(Vector3[] vertices)
    {
        this.vertices = vertices;
        this.face = new List<int[]>();
    }

    public MeshFace(Vector3[] vertices, List<int[]> face)
    {
        this.vertices = vertices;
        this.face = face;
    }

    public void AddTriangle(int[] t) {
        this.face.Add(t);
    }

    public List<int[]> Face { get { return this.face; } }
    public Vector3[] Vertices { get { return this.vertices; } }

    public Vector3 Normal() {
        // Calculate the normal for each triangle and get their average to find the surface normal
        Vector3 normal = Vector3.zero;
        foreach(int[] t in face)
        {
            normal += MeshUtils.CalculateTriangleNormal(new Vector3[] { vertices[t[0]], vertices[t[1]], vertices[t[2]] }).normalized;
        }

        return (normal / face.Count).normalized;
    }
}
