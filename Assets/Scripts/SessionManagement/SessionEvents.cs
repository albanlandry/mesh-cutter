using System;
using UnityEngine;

public class SessionEvents : MonoBehaviour
{
    public static SessionEvents current;
    // Session Events
    private void Awake()
    {
        current = this;
    }

    /*
    public event Action<float, float, float> OnMeshSizeChanged;
    public void MeshSizeChanged(float w, float h, float d) {
        if(OnMeshSizeChanged != null)
        {
            OnMeshSizeChanged(w, h, d);
        }
    }
    */
}
