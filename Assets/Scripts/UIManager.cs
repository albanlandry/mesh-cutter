﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject hierarchyRow;
    [SerializeField] GameObject exposurePanel;
    [SerializeField] Color highlightColor = new Color(255.0f, 145.0f, 0.0f);
    [SerializeField] Color DefaultColor = Color.white;
    [SerializeField] Dropdown dropDown;
    [SerializeField] Text editionMode;

    private HierarchyListModel hierarchy;
    private SelectionManager selectionManager;
    private const string modelValue = "Model: ";
    private const string surfacesValue = "Surface: ";
    private const string exposureValue = "Exposures: ";

    private Dictionary<int, int> materialType;

    private void OnEnable()
    {
        materialType = new Dictionary<int, int>();
        materialType.Add(0, 3);
        materialType.Add(1, 5);
        materialType.Add(2, 10);
    }
    // Start is called before the first frame update
    void Start()
    {
        SessionEvents.current.onModelLoaded += AddToHierarchy;
        SessionEvents.current.onModelUnLoaded += RemoveFromHierarchy;
        // SessionEvents.current.OnModelSelected += SelectRow;
        // SessionEvents.current.OnModelDeselected += DeselectRow;
        SessionEvents.current.OnCutModeEnable += EnableGUIElements;
        SessionEvents.current.OnCutModeDisable += DisableGUIElements;
        // SessionEvents.current.OnSelectionAny += UpdateExposure;
        // SessionEvents.current.OnDeselectionAny += UpdateExposure;
        // SessionEvents.current.OnSelectionAny += AddSelectionToPanel;
        // SessionEvents.current.OnDeselectionAny += removeSelectionToPanel;

        hierarchy = new HierarchyListModel();
        selectionManager = GameObject.Find("MainManager").GetComponent<SelectionManager>();
        editionMode.text = "Selection Mode";
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var item in selectionManager.GetSelection())
        {
            SelectRow(item);
        }
    }

    private void UpdateExposure() {
        string modelNames = "";

        if (selectionManager) {
            List<string> selection = selectionManager.GetSelection();
            MeshExposure exposure = new MeshExposure(0, 0.0f);

            foreach(string selected in selection)
            {
                // Find the corresponding object sessionModel
                MeshExposure exp = GameObject.Find(selected).GetComponent<SessionModel>().GetExposure();

                exposure.Faces = exposure.Faces + exp.Faces;
                exposure.Exposure = exposure.Exposure + exp.Exposure;
                modelNames += selected+", ";
            }

            UpdateExposurePanel(modelValue + modelNames, surfacesValue + exposure.Faces, exposureValue + exposure.Exposure);
        }
    }

    void EnableGUIElements() {
            editionMode.text = "Cutting Mode";
    }

    void DisableGUIElements() {
        editionMode.text = "Selection Mode";
    }

    public void AddSelectionToPanel()
    {
        ClearContentHierarchyContent();
        List<string> names = selectionManager.GetSelection();

        foreach(string name in names)
        {
            AddToHierarchy(name, "");
        }
    }

    private void removeSelectionToPanel()
    {
        List<string> names = selectionManager.GetSelection();

        foreach (string name in names)
        {
            RemoveFromHierarchy(name);
        }
        
    }

    public void UpdateMaterial()
    {
        int type = materialType[dropDown.value];
        Debug.Log("Type: " + type);
        if (selectionManager)
        {
            List<string> selection = selectionManager.GetSelection();
            foreach (string selected in selection)
            {
                MeshExposure exp = GameObject.Find(selected).GetComponent<SessionModel>().GetExposure();
                exp.Material = type;
                exp.Exposure = ExposureCalculator.ComputeExposure(exp.Faces, exp.Material); 
            }

        }

        UpdateExposure();
    }

    void UpdateExposurePanel(string model, string surface, string exposure) {
        UpdateModelValueView("modelValue", model);
        UpdateModelValueView("surfaceValue", surface);
        UpdateModelValueView("exposureValue", exposure);
    }

    void UpdateModelValueView(string name, string value)
    {
        // GameObject.Find(name).GetComponent<Text>().text = value;
    }

    private void OnDisable()
    {
        SessionEvents.current.onModelLoaded -= AddToHierarchy;
        SessionEvents.current.onModelUnLoaded -= RemoveFromHierarchy;
        SessionEvents.current.OnModelSelected -= SelectRow;
        SessionEvents.current.OnModelDeselected -= DeselectRow;
        SessionEvents.current.OnCutModeEnable -= EnableGUIElements;
        SessionEvents.current.OnCutModeDisable -= DisableGUIElements;
        SessionEvents.current.OnSelectionAny -= UpdateExposure;
        SessionEvents.current.OnDeselectionAny -= UpdateExposure;
    }

    private void AddToHierarchy(string id, string parent)
    {
        ClearContentHierarchyContent();
        HierarchyListNode node = new HierarchyListNode();
        node.Id = id;
        node.Parent = parent;

        hierarchy.AddNode(node);
        UpdateHierarchyView();
    }

    private void RemoveFromHierarchy(string id)
    {
        HierarchyListNode node = new HierarchyListNode();
        node.Id = id;
        hierarchy.Remove(node);

        UpdateHierarchyView();
    }

    private void ClearContentHierarchyContent() { 
        foreach(Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void SelectRow(string id) {
        GameObject row = GameObject.Find(id + "Text");

        if (row) {
            SelectTextRow(row);
        }
    }

    void DeselectRow(string id)
    {
        GameObject row = GameObject.Find(id + "Text");

        if (row)
        {
            DeselectTextRow(row);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateHierarchyView()
    {
        ClearContentHierarchyContent();

        for (int i = 0; i < hierarchy.Count; i++) {
            HierarchyListNode n = hierarchy.GetNodeAt(i);

            GameObject row = Instantiate(hierarchyRow, content.transform);

            row.name = n.Id + "Text";
            SetRowTextVal(row, n.Id);

            UpdateChildPosition(row, i, 0.0f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="val"></param>
    void SetRowTextVal(GameObject row, string val) {
        Text text = row.GetComponentInChildren<Text>();
        text.text = val;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    void SelectTextRow(GameObject row)
    {
        Text text = row.GetComponentInChildren<Text>();

        if (text) {
            text.color = highlightColor;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    void DeselectTextRow(GameObject row)
    {
        Text text = row.GetComponentInChildren<Text>();

        if (text)
        {
            text.color = DefaultColor;
        }
    }

    void UpdateChildPosition(GameObject row, int pos, float spacing) {
        RectTransform rect = row.GetComponent<RectTransform>();

        Vector2 scaledRect = Vector2.Scale(rect.rect.size, rect.lossyScale);
        float y = scaledRect.y * pos + spacing;

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - y);
    }

    public static Rect RectransformToScreenSpace(RectTransform transform) {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);

        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);

        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);

        return rect;
    }
}
