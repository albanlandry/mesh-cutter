using UnityEngine;

public class ShowItemDescription : MonoBehaviour
{
    private IRayProvider rayProvider;
    private ISelector selector;

    private Transform selection;
    public GameObject itemDescriptionPrefab; // Prefabs of the item description
    private GameObject itemDescription;
    private bool isInstantiated =  false;
    private void Awake()
    {
        rayProvider = GetComponent<IRayProvider>();
        selector = GetComponent<ISelector>();

    }
    private void Update()
    {
        // Create the ray
        Ray ray = rayProvider.CreateRay();

        // Check if the ray hits a selectable object 
        selector.check(ray);

        // Get the selection 
        selection = selector.GetSelection();

        // Display the UI Element at the selection position if the selection is not null
        if (selection != null && !isInstantiated) {
            // Set the position of the transform on the screen
            itemDescription = Instantiate(itemDescriptionPrefab, this.transform);
            itemDescription.transform.position = Camera.main.WorldToScreenPoint(selection.position);

            isInstantiated = true;
        }

        if (!selection) {
            Destroy(itemDescription);
            isInstantiated = false;
        }
    }
}
