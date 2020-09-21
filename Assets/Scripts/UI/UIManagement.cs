using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    public Button cutBtn;
    public SelectionManager selectionManager;
    // Start is called before the first frame update
    void Start()
    {
        SessionEvents.current.OnSelectionAny += OnSelectionReady; 
        SessionEvents.current.OnDeselectionAny += DeSelectionReady; 
    }

    private void OnDisable()
    {
        SessionEvents.current.OnSelectionAny -= OnSelectionReady;
        SessionEvents.current.OnDeselectionAny -= DeSelectionReady;
    }

    private void OnSelectionReady()
    {
        cutBtn.interactable = true;
    }

    private void DeSelectionReady()
    {
        cutBtn.interactable = false;
    }

    public void CutSelection()
    {
        List<string> selection = selectionManager.GetSelection();
        Cutter.DetachMesh(selection.ToArray());

        // Deselect all the current selection
        selectionManager.DeselectAll();
    }
}
