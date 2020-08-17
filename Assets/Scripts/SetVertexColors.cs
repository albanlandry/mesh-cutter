using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class SetVertexColors : MonoBehaviour
{
    public Color32 m_color;
    private Color32 m_prevColor;
    private List<Color32> m_colorsOut;
    private Mesh m_mesh;
    private List<List<int>> m_triangles;

    void Initialize()
    {
        if (m_mesh == null)
            m_mesh = GetComponent<MeshFilter>().sharedMesh;

        if (m_triangles == null || m_triangles.Count != m_mesh.subMeshCount)
        {
            m_triangles = new List<List<int>>(m_mesh.subMeshCount);
            for (int i = 0; i < m_mesh.subMeshCount; i++)
            {
                m_triangles.Add(new List<int>());
                m_mesh.GetTriangles(m_triangles[i], i);
            }
        }

        if (m_colorsOut == null)
        {
            m_colorsOut = new List<Color32>(new Color32[m_mesh.vertexCount]);
            m_mesh.GetColors(m_colorsOut);
        }

        if (m_colorsOut.Count != m_mesh.vertexCount)
        {
            m_colorsOut = new List<Color32>(new Color32[m_mesh.vertexCount]);
        }
    }

    public void Apply(List<int> element)
    {
        Initialize();

        m_prevColor = m_color;

        // Only set the colors for the selected element indices
        for (int i = 0; i < element.Count; i++)
            m_colorsOut[element[i]] = m_color;
        m_mesh.SetColors(m_colorsOut);
    }

    /// <summary>
    /// Given a single triangle face of vertex indices, 
    /// returns a list of all the vertices of all linked faces.
    /// </summary>
    /// <param name="pickedTriangle">The known triangle to find linked faces from.</param>
    /// <param name="triangles">The index buffer triangle list of all vertices in the mesh.</param>
    /// <param name="isDestructive"></param>
    /// <returns></returns>
    public List<int> GetElement(int[] pickedTriangle, List<int> triangles, bool isDestructive = true)
    {
        // Create the return result list, starting with the current picked face
        List<int> result = new List<int>(pickedTriangle);

        // Iterate through the triangle list index buffer by triangle (iterations of 3)
        for (int i = 0; i < triangles.Count; i += 3)
        {
            // Select the (i)th triangle in the index buffer
            int[] curTriangle = new int[3] { triangles[i], triangles[i + 1], triangles[i + 2] };

            // Check if faces are linked
            if (IsConnected(curTriangle, pickedTriangle))
            {
                if (isDestructive)
                {
                    triangles[i] = -1;
                    triangles[i + 1] = -1;
                    triangles[i + 2] = -1;
                }

                // Recursively add all the linked faces to the result
                result.AddRange(GetElement(curTriangle, triangles));
            }
        }

        return result;
    }

    /// <summary>
    /// Given two faces, return whether they share any common vertices.
    /// </summary>
    /// <param name="faceA">Face represented as array of vertex indices.</param>
    /// <param name="faceB">Face represented as array of vertex indices.</param>
    /// <returns>bool - whether the faces are connected. </returns>
    bool IsConnected(int[] faceA, int[] faceB)
    {
        for (int i = 0; i < faceA.Length; i++)
            for (int j = 0; j < faceB.Length; j++)
                if (faceA[i] == faceB[j])
                    return true;
        return false;
    }

}
