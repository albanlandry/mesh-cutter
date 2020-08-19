using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshExposure {
    private int faces;
    private float exposure;
    private int material;

    public MeshExposure()
    {
        faces = 0;
        exposure = 0;
    }

    public MeshExposure (int _faces) {
        this.faces = _faces;
        this.exposure = 0.0f;
    }

    public MeshExposure (int _faces, float _exposure) {
        this.faces = _faces;
        this.exposure = _exposure;
    }

    public int Faces { get; set; }
    public int Material { get; set; }
    public float Exposure { get; set; }
}