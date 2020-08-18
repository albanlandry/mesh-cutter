using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemDescription2 : MonoBehaviour
{
    private IRayProvider rayProvider;
    private ISelector selector;
    private Transform selection;
    private GameObject itemDescription;

    public RectTransform parentUI; // The parent to which we need to attach the UI
    public GameObject itemDescriptionPrefab; // Prefabs of the item description

    private void Awake()
    {
        rayProvider = GetComponent<IRayProvider>();
        selector = GetComponent<ISelector>();

    }

    private void OnMouseEnter()
    {

        // Create the ray
        Ray ray = rayProvider.CreateRay();

        // Check if the ray hits a selectable object 
        selector.check(ray);

        // Get the selection 
        selection = selector.GetSelection();

        // Display the UI Element at the selection position if the selection is not null
        if (selection != null)
        {
            // Set the position of the transform on the screen
            itemDescription = Instantiate(itemDescriptionPrefab, parentUI);
            itemDescription.transform.position = Camera.main.WorldToScreenPoint(selection.position);
        }
    }

    private void OnMouseDrag()
    {
        if (itemDescription != null)
        {
            itemDescription.transform.position = Camera.main.WorldToScreenPoint(selection.position);
        }
    }

    private void OnMouseExit()
    {
        if (itemDescription != null)
        {
            Destroy(itemDescription);
        }
    }

    public void OnDestroy()
    {
        
    }
}
