using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    List<string> selected;
    public RectTransform selectionBox;
    private Vector2 startPos;

    private IRayProvider rayProvider;
    private ISelector selector;
    private Shader selectionShader;
    private Shader standardShader;
    bool isCutting = false;
    bool isBoxSelection = false;
    bool isBoxSelectionEnabled = true;

    void Start()
    {
        SessionEvents.current.OnSelectionClear += ClearSelection;
        SessionEvents.current.OnCutModeEnable += CutEnable;
        SessionEvents.current.OnCutModeDisable += CutDisable;
        SessionEvents.current.onModelUnLoaded += removeFromSelection;
        SessionEvents.current.OnItemDragAny += DisableBoxSelection;
        SessionEvents.current.OnItemDragStopAny += EnableBoxSelection;

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
        SessionEvents.current.OnItemDragAny -= DisableBoxSelection;
        SessionEvents.current.OnItemDragStopAny -= EnableBoxSelection;
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
        if (!isCutting && isBoxSelectionEnabled) { 
            Transform selection;

            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                DeselectAll();
                ReleaseSelectionBox();

                if (isBoxSelection)
                {
                    isBoxSelection = false;
                }
                else
                {
                    // Create the ray
                    Ray ray = rayProvider.CreateRay();

                    // Check if the ray hits a selectable object 
                    selector.check(ray);

                    // Get the selection 
                    selection = selector.GetSelection();

                    // We add the selection into the selection list if it is not already present
                    if (selection != null && !selection.tag.Equals("NOT_SELECTABLE"))
                    {
                        string select = selection.gameObject.name;
                        // Debug.Log("selection: " + select);
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
                                Debug.Log("Selection name: " + select);

                                if (selection.tag.Equals("Selectable"))
                                {
                                    SelectModelFromParent(selection.gameObject);
                                }
                                else
                                {
                                    SelectModel(selection.gameObject);
                                }

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
                    else
                    {
                        DeselectAll();
                    }
                }
            }

            // Mouse held down
            if (Input.GetMouseButton(0))
            {
                UpdateSelectionBox(Input.mousePosition);
            }
        } // isCutting
    }

    void UpdateSelectionBox(Vector2 mousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = mousePos.x - startPos.x;
        float height = mousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        // Disabling the selection box
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition +  (selectionBox.sizeDelta / 2);

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Sliceable");

        foreach(GameObject obj in objects)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectModel(obj);
                isBoxSelection = true;
            }
        }
    }

    public void SelectModel(GameObject obj)
    {
        selected.Add(obj.name);
        SessionEvents.current.SelectionAny();
        SessionEvents.current.ModelSelected(obj.name);
        UpdateMeshShader(obj, selectionShader);
    }

    public void SelectModelFromParent(GameObject obj)
    {
        foreach(Transform child in obj.transform)
        {
            SelectModel(child.gameObject);
        }
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
    /// Deselect all the currently selected objects
    /// </summary>
    public void DeselectAll()
    {
        foreach(string name in GetSelection().ToArray())
        {
            DeselectModel(GameObject.Find(name));
        }
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

    public void DisableBoxSelection()
    {
        isBoxSelectionEnabled = false;
    }
    public void EnableBoxSelection()
    {
        isBoxSelectionEnabled = true;
    }
}
