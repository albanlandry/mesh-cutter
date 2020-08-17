using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCounter : MonoBehaviour
{
    int surfaces = 0;
    float bias = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        surfaces = MeshUtils.CountSurfaces(mesh, bias);

        Debug.Log("Surfaces count: "+surfaces);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
