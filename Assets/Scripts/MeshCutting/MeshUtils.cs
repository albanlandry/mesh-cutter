using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils
{
    /// <summary>
    /// Find center of polygon by averaging vertices
    /// </summary>
    public static Vector3 FindCenter(List<Vector3> pairs)
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        for (int i = 0; i < pairs.Count; i += 2)
        {
            center += pairs[i];
            count++;
        }

        return center / count;
    }


    /// <summary>
    /// Reorder a list of pairs of vectors (one dimension list where i and i + 1 defines a line segment)
    /// So that it forms a closed polygon 
    /// </summary>
    public static void ReorderList(List<Vector3> pairs)
    {
        int nbFaces = 0;
        int faceStart = 0;
        int i = 0;

        while (i < pairs.Count)
        {
            // Find next adjacent edge
            for (int j = i + 2; j < pairs.Count; j += 2)
            {
                if (pairs[j] == pairs[i + 1])
                {
                    // Put j at i+2
                    SwitchPairs(pairs, i + 2, j);
                    break;
                }
            }


            if (i + 3 >= pairs.Count)
            {
                // Why does this happen?
                Debug.Log("Huh?");
                break;
            }
            else if (pairs[i + 3] == pairs[faceStart])
            {
                // A face is complete.
                nbFaces++;
                i += 4;
                faceStart = i;
            }
            else
            {
                i += 2;
            }
        }
    }

    private static void SwitchPairs(List<Vector3> pairs, int pos1, int pos2)
    {
        if (pos1 == pos2) return;

        Vector3 temp1 = pairs[pos1];
        Vector3 temp2 = pairs[pos1 + 1];
        pairs[pos1] = pairs[pos2];
        pairs[pos1 + 1] = pairs[pos2 + 1];
        pairs[pos2] = temp1;
        pairs[pos2 + 1] = temp2;
    }

    /// <summary>
    /// Find the number of surfaces of a mesh
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="bias"></param>
    /// <returns></returns>
    /// 
    /*
    public static int CountSurfaces(Mesh mesh, float bias)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        int count = 0; // Faces count

        // Debug.Log("Triangles Count: " +triangles.Length);

        for (int i = 0; i < triangles.Length;  i+=3) {
            bool foundFace = false;
            int[] currentFace = {
                triangles[i],
                triangles[i + 1],
                triangles[i + 2]
            };

            // Loop through alf the faces to find those with similar normals
            for (int j = i+3; j < triangles.Length; j += 3) {
                int[] triangleFace = {
                    triangles[j],
                    triangles[j + 1],
                    triangles[j + 2]
                };

                // We check the normal only if the faces are connected
                if (IsConnected(currentFace, triangleFace)) {

                    Debug.Log("Connected");

                    Vector3[] currentTriangle = {
                        normals[currentFace[0]],
                        normals[currentFace[1]],
                        normals[currentFace[2]]
                    };

                    Vector3[] triangle = {
                        normals[triangleFace[0]],
                        normals[triangleFace[1]],
                        normals[triangleFace[2]]
                    };


                    // Check whether the normals are similar
                    Vector3 n1 = TriangleNormal(currentTriangle[0], currentTriangle[1], currentTriangle[2]);
                    Vector3 n2 = TriangleNormal(triangle[0], triangle[1], triangle[2]);

                    Debug.Log("Normales: n1: "+n1+", n2: "+n2+", Dot: "+ Vector3.Dot(n1, n2)+", Angle: "+Vector3.Angle(n1, n2));

                    if (Vector3.Dot(n1, n2) == 1)
                    {
                        foundFace = true;
                    }
                }
            }
            
            // If we found a face
            if (foundFace)
            {
                count++;
            }
            // Debug.Log("Triangle: "+ triangles[i]+ ", "+ triangles[i+1]+ ", "+ triangles[i+2]);
        }

        return count;
    }
    */
    public static int CountSurfaces(Mesh mesh, float bias)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        int count = 0; // Faces count

        // Debug.Log("Triangles Count: " +triangles.Length);
        List<MeshFace> faces = GetFaces(triangles, vertices);

        Debug.Log("Faces number: " + faces.Count);

        /*
        int[] currentFace = {
                triangles[0],
                triangles[1],
                triangles[2]
        };
        MeshFace face = GetFace(currentFace, triangles, vertices);
        Debug.Log("Face number: " + face.Face.Count);

        for (int i = 3; i < triangles.Length; i += 3)
        {
            int[] triangleFace = {
                    triangles[i],
                    triangles[i + 1],
                    triangles[i + 2]
            };

            if (IsConnected(currentFace, triangleFace)) {
                Vector3[] currentTriangle = {
                        vertices[currentFace[0]],
                        vertices[currentFace[1]],
                        vertices[currentFace[2]]
                    };

                Vector3[] triangle = {
                        vertices[triangleFace[0]],
                        vertices[triangleFace[1]],
                        vertices[triangleFace[2]]
                    };

                // Compute all the given normales
                Vector3 n1 = CalculateTriangleNormal(currentTriangle);
                Vector3 n2 = CalculateTriangleNormal(triangle);
                // Vector3 n2 = CalculateTriangleNormal();

                Debug.Log("Normales: " + n1 + ", " + n2+", Dot: "+ Vector3.Dot(n1, n2)+", cos: "+Mathf.Cos(0.0f));

                // Check whether the 2 triangles form a face
                if (HasFormedFace(n1, n2, 18.0f)) {
                    count++;
                }
            }

            // Switch to the next triangle
            currentFace = triangleFace;
        }
        */

        return count;
    }

    /// <summary>
    ///  Find the normal of a triangle mesh
    /// </summary>
    /// <param name="mesh"></param>
    public static Vector3 TriangleNormal(Vector3 v1, Vector3 v2, Vector3 v3) 
    {
        return (v1 + v2 + v3) / 3;
    }

    /// <summary>
    /// Check whether faceA and faceB are connected by 2 vertices
    /// </summary>
    /// <param name="faceA"></param>
    /// <param name="faceB"></param>
    /// <returns></returns>
    public static bool IsConnected(int[] faceA, int[] faceB)
    {
        int match = 0;
        for (int i = 0; i < faceA.Length; i++)
            for (int j = 0; j < faceB.Length; j++)
                if (faceA[i] == faceB[j])
                    match++;

        return match >= 2;
    }

    /// <summary>
    /// Compute triangle normal
    /// </summary>
    /// <param name="triangle"></param>
    /// <returns></returns>
    public static Vector3 CalculateTriangleNormal(Vector3[] triangle) {
        Vector3 u = triangle[1] - triangle[0];
        Vector3 v = triangle[2] - triangle[0];

        return Vector3.Cross(u, v).normalized;
    }

    /// <summary>
    /// Find all the triangles which belong ti the same face
    /// </summary>
    /// <param name="triangle"></param>
    /// <param name="triangles"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static MeshFace GetFace(int[] currentFace, int[] triangles, Vector3[] vertices) {
        MeshFace face = new MeshFace(vertices);

        for (int i = 0; i < triangles.Length; i += 3) {
            int[] triangleFace = {
                    triangles[i],
                    triangles[i + 1],
                    triangles[i + 2]
            };

            if (IsConnected(currentFace, triangleFace))
            {
                Vector3[] currentTriangle = {
                        vertices[currentFace[0]],
                        vertices[currentFace[1]],
                        vertices[currentFace[2]]
                    };

                Vector3[] triangle = {
                        vertices[triangleFace[0]],
                        vertices[triangleFace[1]],
                        vertices[triangleFace[2]]
                    };

                // Compute all the given normales
                Vector3 n1 = CalculateTriangleNormal(currentTriangle);
                Vector3 n2 = CalculateTriangleNormal(triangle);

                // Check whether the 2 triangles form a face
                if (HasFormedFace(n1, n2, 0.0f))
                {
                    face.AddTriangle(triangleFace);
                }

            }
        }

        return face;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="triangles"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static List<MeshFace> GetFaces(int[] triangles, Vector3[] vertices) {
        List<MeshFace> faces = new List<MeshFace>();

        // Find all the connected faces of the mesh
        for (int i = 0; i < triangles.Length;)
        {
            int[] triangleFace = {
                        triangles[i],
                        triangles[i + 1],
                        triangles[i + 2]
            };

            MeshFace face = GetFace(triangleFace, triangles, vertices);

            if (face.Face.Count > 0)
            {
                faces.Add(face);
                i += 3 * face.Face.Count;
            }
            else
            {
                i +=3;
            }
        }

        return faces;
    }

    /// <summary>
    /// Check whether 2 triangles form a face
    /// </summary>
    /// <param name="n1"></param>
    /// <param name="n2"></param>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static bool HasFormedFace(Vector3 n1, Vector3 n2, float degree)
    {
        return Vector3.Dot(n1, n2) >= Mathf.Cos(degree);
    }
}
