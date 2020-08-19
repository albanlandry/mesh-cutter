using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    List<string> selected;
    private IRayProvider rayProvider;
    private ISelector selector;
    private Shader selectionShader;
    private Shader standardShader;

    void Start()
    {
        SessionEvents.current.OnSelectionClear += ClearSelection;

        selected = new List<string>();
        rayProvider = GetComponent<IRayProvider>();
        selector = GetComponent<ISelector>();
        selectionShader = Shader.Find("Unlit/Outline");
        standardShader = Shader.Find("Standard");
    }

    private void OnDisable()
    {
        SessionEvents.current.OnSelectionClear -= ClearSelection;
    }

    // Update is called once per frame
    void Update()
    {
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
                        selected.Add(select);
                        SessionEvents.current.SelectionAny();
                        SessionEvents.current.ModelSelected(select);
                        UpdateMeshShader(selection.gameObject, selectionShader);
                    }
                    else // Deselect to delete from the list of selected item
                    {
                        selected.Remove(select);
                        SessionEvents.current.DeselectionAny();
                        SessionEvents.current.ModelDeselected(select);
                        UpdateMeshShader(selection.gameObject, standardShader);
                    }
                }
            }
        }
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
        foreach(string select in selected)
        {
            SessionEvents.current.ModelDeselected(select);
        }

        selected.Clear();
    }
}
