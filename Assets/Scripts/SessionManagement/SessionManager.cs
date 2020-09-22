using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager current;
    SelectionManager selectionManager;
    bool inCutMode = false;

    private void OnEnable()
    {
        current = this;
    }

    private void Start()
    {
        InitSession();
        selectionManager = GetComponent<SelectionManager>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitSession() {
        GameObject[] sliceables = GameObject.FindGameObjectsWithTag("Sliceable");

        foreach(GameObject obj in sliceables)
        {
            if (obj.activeSelf)
            {
                SessionModel model = obj.GetComponent<SessionModel>();

                if (model) {
                    LoadModel(model, null);
                }
            }
        }
    }

    public void UnloadModel(string name)
    {
        SessionEvents.current.ModelUnLoaded(name);
    }

    public void LoadModel(SessionModel model, string parent) {
        Transform tr = model.GetComponent<Transform>();
        SessionEvents.current.ModelLoaded(tr.gameObject.name, parent);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.C)) {
            // if (selectionManager.SelectionCount() > 0) {
                EnableCutMode();
            // }
        }
        else
        {
            DisableCutMode();
        }

        if (Input.GetKey(KeyCode.B))
        {
            SessionEvents.current.EnableSelection();
        }
        else
        {
            SessionEvents.current.DisableSelection();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            selectionManager.ClearSelection();
        }
    }

    public void DisableCutMode()
    {
        SessionEvents.current.CutModeDisabled();
        inCutMode = false;
    }

    public void EnableCutMode() {
        SessionEvents.current.CutModeEnabled();
        inCutMode = true;
    }
}
