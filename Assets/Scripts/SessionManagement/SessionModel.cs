using System;
using UnityEngine;

public class SessionModel : MonoBehaviour
{
    private MeshedModel model;
    private MeshExposure exposure;

    private void Awake()
    {
        model = new MeshedModel(50, 200, 50, 1);
        exposure = new MeshExposure();
    }

    private void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        exposure.Faces = MeshUtils.CountSurfaces(mesh, 0.0f);
        exposure.Material = 3;
        exposure.Exposure = ExposureCalculator.ComputeExposure(exposure.Faces, exposure.Material);
    }

    public MeshExposure GetExposure() {
        /**
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        exposure.Faces = MeshUtils.CountSurfaces(mesh, 0.0f);
        exposure.Exposure = ExposureCalculator.ComputeExposure(exposure.Faces, 1);
        */
        return exposure;
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
