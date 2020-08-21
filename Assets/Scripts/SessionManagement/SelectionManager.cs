﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    List<string> selected;
    private IRayProvider rayProvider;
    private ISelector selector;
    private Shader selectionShader;
    private Shader standardShader;
    bool isCutting = false;

    void Start()
    {
        SessionEvents.current.OnSelectionClear += ClearSelection;
        SessionEvents.current.OnCutModeEnable += CutEnable;
        SessionEvents.current.OnCutModeDisable += CutDisable;
        SessionEvents.current.onModelUnLoaded += removeFromSelection;

        selected = new List<string>();
        rayProvider = GetComponent<IRayProvider>();
        selector = GetComponent<ISelector>();
        selectionShader = Shader.Find("Unlit/Outline");
        standardShader = Shader.Find("Standard");
    }

    private void OnDisable()
    {
        SessionEvents.current.OnSelectionClear -= ClearSelection;
        SessionEvents.current.OnCutModeEnable -= CutEnable;
        SessionEvents.current.OnCutModeDisable -= CutDisable;
        SessionEvents.current.onModelUnLoaded -= removeFromSelection;
    }

    void CutEnable() {
        isCutting = true;
    }

    void CutDisable() {
        isCutting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCutting) { 
            Transform selection;

            if (Input.GetMouseButtonUp(0))
            {
                // Create the ray
                Ray ray = rayProvider.CreateRay();

                // Check if the ray hits a selectable object 
                selector.check(ray);

                // Get the selection 
                selection = selector.GetSelection();

                // We add the selection into the selection list if it is not already present
                if (selection != null)
                {
                    string select = selection.gameObject.name;

                    if (select != null)
                    {
                        if (!selected.Contains(select))
                        {
                            /*
                            selected.Add(select);
                            SessionEvents.current.SelectionAny();
                            SessionEvents.current.ModelSelected(select);
                            UpdateMeshShader(selection.gameObject, selectionShader);
                            */
                            SelectModel(selection.gameObject);
                        }
                        else // Deselect to delete from the list of selected item
                        {
                            /*
                            selected.Remove(select);
                            SessionEvents.current.DeselectionAny();
                            SessionEvents.current.ModelDeselected(select);
                            UpdateMeshShader(selection.gameObject, standardShader);
                            */
                            DeselectModel(selection.gameObject);
                        }
                    }
                }
            }
        } // isCutting
    }

    public void SelectModel(GameObject obj)
    {
        selected.Add(obj.name);
        SessionEvents.current.SelectionAny();
        SessionEvents.current.ModelSelected(obj.name);
        UpdateMeshShader(obj, selectionShader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void DeselectModel(GameObject obj)
    {
        selected.Remove(obj.name);
        SessionEvents.current.DeselectionAny();
        SessionEvents.current.ModelDeselected(obj.name);
        UpdateMeshShader(obj, standardShader);
    }

    /// <summary>
    /// Callback to execute when a model is unloaded from the session
    /// </summary>
    /// <param name="selected"></param>
    private void removeFromSelection(string selection)
    {
        selected.Remove(selection);
    }

    /// <summary>
    /// Update the shader of the selected gameobject
    /// </summary>
    /// <param name="obj"></param>
    void UpdateMeshShader(GameObject obj, Shader shader) {
        Renderer rdr = obj.GetComponent<Renderer>();
        rdr.material.shader = shader;
    }

    public List<string> GetSelection() {
        return selected;
    }

    public int SelectionCount() {
        return selected.Count;
    }

    public void ClearSelection() {
        var selection = new List<string>(GetSelection());
        foreach(string select in selection)
        {
            DeselectModel(GameObject.Find(select));
        }

        selected.Clear();
    }
}
