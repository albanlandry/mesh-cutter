using UnityEngine;
using UnityEngine.UI;

public class ModelEditor : MonoBehaviour
{
    public GameObject PanelEditor;
    public RectTransform editorSize;
    public Camera cameraView;
    public const int EDITOR_LAYER = 8;
    GameObject mesh = null;
    // [SerializeField] GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        // Mesh events
        SessionEvents.current.onMeshModelClick += CenterModel;

        // Camera events
        SessionEvents.current.onCameraDisable += CleanEditor;
        editorSize = PanelEditor.GetComponent<RectTransform>();
        /*
        Debug.Log("Size" + editorSize.rect);
        Debug.Log("Center" + editorSize.rect.center);
        Debug.Log("Screen Position" + editorSize.position);
        Debug.Log("World Position" + cameraView.ScreenToWorldPoint(editorSize.position));
        Debug.Log("Camera Viewport" + cameraView.pixelRect);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CenterModel (GameObject model){
        // Create a clone of the object to display
        // And set its layer to the second camera
        if (mesh == null) {
            mesh = Instantiate(model, transform);
            mesh.AddComponent<ObjectRotator>();
            SetParentLayerWithChildren(mesh, EDITOR_LAYER);

            // Get the boundaries of the panel - editorSize variable
            // Compute the center of the panel
            // Set the panel a the computed center position
            Rect cameraBounds = cameraView.pixelRect;
            Vector3 centerPoint = cameraView.ScreenToWorldPoint(new Vector3(editorSize.rect.width / 2, editorSize.rect.height / 2, mesh.transform.position.z));
            mesh.transform.position = centerPoint;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    void SetParentLayerWithChildren(GameObject parent, int layer)
    {
        mesh.layer = layer;

        int i = 0;
        foreach (MeshModel child in transform.GetComponentsInChildren<MeshModel>()) {
            child.gameObject.layer = layer;
            i++;
        }
        Debug.Log("CHILDREN COUNT: "+i);
    }

    /// <summary>
    /// Clean the editor of any unnecessary element when the editor is closed
    /// </summary>
    /// <param name="id">The id of the camera to handle</param>
    void CleanEditor(int id) {
        if (id == 1) {
            // Remove mesh if not null
            if (mesh) {
                Destroy(mesh);
            }
        }
    }
}
