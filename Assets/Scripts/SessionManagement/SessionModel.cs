using System;
using UnityEngine;

public class SessionModel : MonoBehaviour
{
    private MeshedModel model;

    private void Awake()
    {
        model = new MeshedModel(50, 200, 50, 1);
    }

    /// <summary>
    /// Update the size of the associated MeshModel
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    private void SizeChanged(float  width, float height, float depth) {
        model.Width = width;
        model.Height = height;
        model.Depth = depth;
    }

    public void UpdateSize(float width, float height, float depth)
    {
        model.Width = width;
        model.Height = height;
        model.Depth = depth;

        Debug.Log("w: " + width+", h: "+height+", depth: "+depth);

        Debug.Log("Polygon Faces: "+ (transform.GetComponent<MeshFilter>().mesh.triangles.Length / 3 ));
    }
}
