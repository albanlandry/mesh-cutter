using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject hierarchyRow;
    [SerializeField] Color highlightColor = new Color(255.0f, 145.0f, 0.0f);
    [SerializeField] Color DefaultColor = Color.white;

    private HierarchyListModel hierarchy;

    private void OnEnable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        SessionEvents.current.onModelLoaded += AddToHierarchy;
        SessionEvents.current.OnModelSelected += SelectRow;
        SessionEvents.current.OnModelDeselected += DeselectRow;

        hierarchy = new HierarchyListModel();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnDisable()
    {
        SessionEvents.current.onModelLoaded -= AddToHierarchy;
        SessionEvents.current.OnModelSelected -= SelectRow;
        SessionEvents.current.OnModelDeselected -= DeselectRow;
    }

    private void AddToHierarchy(string id, string parent)
    {
        HierarchyListNode node = new HierarchyListNode();
        node.Id = id;
        node.Parent = parent;

        hierarchy.AddNode(node);
        UpdateHierarchyView(node);
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
    private void UpdateHierarchyView(HierarchyListNode node)
    {
        int pos = hierarchy.Count - 1;
        GameObject row = Instantiate(hierarchyRow, content.transform);

        row.name = node.Id+"Text";
        SetRowTextVal(row, node.Id);

        UpdateChildPosition(row, pos, 0.0f);
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
