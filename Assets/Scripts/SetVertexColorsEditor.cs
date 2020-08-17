using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetVertexColors))]
public class SetVertexColorsEditor : Editor
{
    List<List<int>> m_triangles;
    List<Vector3> m_vertices;

    public void OnSceneGUI()
    {

        if (Event.current.type != EventType.MouseDown)
            return;

        if (Event.current.button != 0)
            return;

        SetVertexColors _target = (SetVertexColors)target;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        MeshFilter mf = _target.GetComponent(typeof(MeshFilter)) as MeshFilter;
        Mesh mesh = mf.sharedMesh;

        // Initialize temp buffer data if it doesn't exist
        if (m_triangles == null)
        {
            m_triangles = new List<List<int>>(mesh.vertexCount / 3);
            for (int i = 0; i < mesh.subMeshCount; i++)
                m_triangles.Add(new List<int>());
        }
        if (m_vertices == null)
            m_vertices = new List<Vector3>(mesh.vertexCount);

        // Retrieve vertex positions from mesh. Using List prevents subsequent copying
        mesh.GetVertices(m_vertices);

        // Accumulate all of the faces an infinite ray may penetrate.
        List<Vector4> candidates = new List<Vector4>();
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            List<int> triangles = m_triangles[i];
            mesh.GetTriangles(triangles, i);

            for (int j = 0; j < triangles.Count; j += 3)
            {
                Vector3 p1 = m_vertices[triangles[j]];
                Vector3 p2 = m_vertices[triangles[j + 1]];
                Vector3 p3 = m_vertices[triangles[j + 2]];
                p1 = _target.transform.TransformPoint(p1);
                p2 = _target.transform.TransformPoint(p2);
                p3 = _target.transform.TransformPoint(p3);
                float dist = Intersect(p1, p2, p3, ray.origin, Vector3.Normalize(ray.direction));

                if (dist >= 0)
                    candidates.Add(new Vector4(triangles[j], triangles[j + 1], triangles[j + 2], dist));
            }
        }

        // If no intersections occured, user did not click on the mesh
        if (candidates.Count == 0)
            return;

        // Sort the candidate faces for the closest one, which is the intuitivey picked face
        Vector4 best = new Vector4(0, 0, 0, float.MaxValue);
        for (int j = 0; j < candidates.Count; j++)
            if (candidates[j].w < best.w)
                best = candidates[j];

        int[] pickedFace = new int[] { (int)best.x, (int)best.y, (int)best.z };
        List<int> element = _target.GetElement(pickedFace, new List<int>(mesh.triangles), false);

        // Calls the GetElement function, as well as setting vertex colors to the picked element.
        _target.Apply(element);

        Event.current.Use();
    }

    /// <summary>
    /// Checks if the specified ray hits the triagnlge descibed by p1, p2 and p3.
    /// Möller–Trumbore ray-triangle intersection algorithm implementation.
    /// </summary>

    /// <param name="V1">Vertex 1 of the triangle.</param>
    /// <param name="V2">Vertex 2 of the triangle.</param>
    /// <param name="V3">Vertex 3 of the triangle.</param>
    /// <param name="ray">The ray to test hit for.</param>
    /// <returns><c>true</c> when the ray hits the triangle, otherwise <c>false</c></returns>
    public static float Intersect(Vector3 V1, Vector3 V2, Vector3 V3, Vector3 O, Vector3 D, bool backfaceCulling = true)
    {
        Vector3 e1, e2; //Edge1, Edge2
        Vector3 P, Q, T;
        float det, inv_det;
        float t, u, v;

        //Find vectors for two edges sharing V1
        e1 = V2 - V1;
        e2 = V3 - V1;

        //Begin calculating determinant - also used to calculate u parameter
        P = Vector3.Cross(D, e2);

        //if determinant is near zero, ray lies in plane of triangle or ray is parallel to plane of triangle
        det = Vector3.Dot(e1, P);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        // if the determinant is negative the triangle is backfacing
        // if the determinant is close to 0, the ray misses the triangle
        if (backfaceCulling)
            if (det < Mathf.Epsilon) return -1f;
            else
            if (Mathf.Abs(det) < Mathf.Epsilon) return -1f;

        inv_det = 1.0f / det;

        //calculate distance from V1 to ray origin
        T = O - V1;

        //Calculate u parameter and test bound
        u = Vector3.Dot(T, P) * inv_det;

        //The intersection lies outside of the triangle
        if (u < 0f || u > 1f) return -1f;

        //Prepare to test v parameter
        Q = Vector3.Cross(T, e1);

        //Calculate V parameter and test bound
        v = Vector3.Dot(D, Q) * inv_det;

        //The intersection lies outside of the triangle
        if (v < 0f || u + v > 1f) return -1f;


        t = Vector3.Dot(e2, Q) * inv_det;
        return t;
    }

}