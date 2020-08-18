﻿using System;
using UnityEngine;

public class SessionEvents : MonoBehaviour
{
    public static SessionEvents current;
    // Session Events
    private void OnEnable()
    {
        current = this;
    }

    public event Action onSessionStart;
    public event Action onSessionPause;
    public void SessionStart() { 
        if(onSessionStart != null)
        {
            onSessionStart();
        }
    }

    public void SessionPause()
    {
        if (onSessionPause != null)
        {
            onSessionPause();
        }
    }

    /** ALL EVENTS RELATED TO A MODEL IN THE SCENE */
    public event Action<GameObject> onMeshModelClick;
    public event Action onModelClick;

    public void MeshModelClick(GameObject mesh)
    {
        ModelClick();
        if (onMeshModelClick != null) {
            Debug.Log("Mesh Model Click");
            onMeshModelClick(mesh);
        }
    }

    public void ModelClick() {
        
        if (onModelClick != null)
        {
            Debug.Log("Model Click");
            onModelClick();
        }
    }

    /** ALL EVENTS RELATED TO A CAMERA MANAGEMENT IN THE SCENE */
    public event Action<int> onCameraDisable;
    public event Action<int> onCameraEnable;
    public void CameraDisable(int id) {
        if (onCameraDisable != null) {
            onCameraDisable(id);
        }
    }

    public void CameraEnable(int id)
    {
        if (onCameraEnable != null)
        {
            onCameraEnable(id);
        }
    }

    /** EVENT RELATED TO OBJECT MANAGEMENT */
    public event Action<String, String> onModelLoaded;
    public event Action<String, String> onModelUnLoaded;
    public void ModelLoaded(String id, String parent)
    {
        if (onModelLoaded != null) {
            onModelLoaded(id, parent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parent"></param>
    public void ModelUnLoaded(String id, String parent)
    {
        if (onModelUnLoaded != null)
        {
            onModelUnLoaded(id, parent);
        }
    }

    // Selection of an ibject in session
    public event Action<String> OnModelSelected;
    public event Action<String> OnModelDeselected;
    public void ModelSelected(string id) {
        if (OnModelSelected != null) {
            OnModelSelected(id);
        }
    }

    public void ModelDeselected(string id)
    {
        if (OnModelDeselected != null)
        {
            OnModelDeselected(id);
        }
    }

    public event Action OnCutModeEnable;
    public event Action OnCutModeDisable;
    public void CutModeEnabled() 
    {
        if (OnCutModeEnable != null) {
            OnCutModeEnable();
        }
    }

    public void CutModeDisabled()
    {
        if (OnCutModeDisable != null)
        {
            OnCutModeDisable();
        }
    }
}
