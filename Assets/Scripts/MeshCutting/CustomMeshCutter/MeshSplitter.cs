using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSplitter : MonoBehaviour
{
    private IRayProvider rayProvider;
    private ISelector selector;
    private Shader selectionShader;
    private Shader standardShader;

    bool dragging, isCutting;
    Vector3 start;
    Vector3 end;
    Camera cam;

    Vector3 cutDirection;
    Transform selection; // The last object that inters
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        dragging = false;

        rayProvider = GetComponent<IRayProvider>();
        selector = GetComponent<ISelector>();
        selectionShader = Shader.Find("Unlit/Outline");
        standardShader = Shader.Find("Standard");
        SessionEvents.current.OnCutModeEnable += CutEnable;
        SessionEvents.current.OnCutModeDisable += CutDisable;
    }

    // Update is called once per frame
    void Update()
    {

        if (!dragging && Input.GetMouseButtonDown(0))
        {
            start = cam.ScreenToViewportPoint(Input.mousePosition);
            dragging = true;
        }

        if (dragging)
        {
            end = cam.ScreenToViewportPoint(Input.mousePosition);

            //* WE FIND THE OBJECT THAT INTERSECT WITH THE CUT *****************
            // Create the ray
            Ray ray = rayProvider.CreateRay();
            // Debug.Log(ray);

            // Check if the ray hits a selectable object 
            selector.check(ray);
            // Get the selection 
            if(selector.GetSelection() != null)
                selection = selector.GetSelection();
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            // Finished dragging. We compute the direction of the vector formed by start and end
            end = cam.ScreenToViewportPoint(Input.mousePosition);
            dragging = false;
            cutDirection = end - start;

            // Debug.Log("Direction: " + cutDirection);
            if(selection != null)
            {
                // Debug.Log("Selection: " + selection.gameObject.transform + ", " + selection.name);
                // var coeffs = ParallelismCoefficient(selection, cutDirection, 0.0f);

                /*
                GameObject plane = GameObject.Find("PlaneCutter");
                plane.transform.position = selection.position;
                plane.transform.rotation = Quaternion.LookRotation(cutDirection);
                */
                if (isCutting)
                {
                    Debug.Log("Selection: " + selection);
                    if(selection.parent != null )
                        Cutter.SplitOBJ(selection.parent.gameObject, selection, cutDirection);
                    else
                        Cutter.SplitOBJ(selection.gameObject, selection, cutDirection);
                }
            }
        }

        // Separate the object if we do not drag anymore and the 

    }

    private void OnDisable()
    {
        SessionEvents.current.OnCutModeEnable -= CutEnable;
        SessionEvents.current.OnCutModeDisable -= CutDisable;
    }

    void CutEnable()
    {
        isCutting = true;
    }

    void CutDisable()
    {
        isCutting = false;
    }
}
