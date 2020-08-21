using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceManager : MonoBehaviour
{
    ScreenLineRenderer lineRenderer;
    MouseSlice slice;
    public SelectionManager selectionManager;
    public string selected;
    // Start is called before the first frame update
    void Start()
    {
        SessionEvents.current.OnCutModeEnable += EnableCut;
        SessionEvents.current.OnCutModeDisable += DisableCut;
        lineRenderer = GetComponent<ScreenLineRenderer>();
        slice = GetComponent<MouseSlice>();
    }

    void EnableCut() {
        List<string> selection = selectionManager.GetSelection();

        /*
        if (selection.Count > 1) {
            Debug.Log("Can only cut one mesh at the time");
            return;
        }
        */

        if (selection == null || selection.Count <= 0 ) {
            Debug.Log("NO MESH SELECTED");
            return;
        }

        if (selection.Count > 0)
        {
            lineRenderer.enabled = true;
            slice.enabled = true;
            selected = selection[0];
        }
    }

    void DisableCut()
    {
        selected = null;
        lineRenderer.enabled = false;
        slice.enabled = false;
    }
}
