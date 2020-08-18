using UnityEngine;

public class SessionManager : MonoBehaviour
{
    bool inCutMode = false;
    private void Start()
    {
        InitSession();
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
                SessionEvents.current.ModelLoaded(obj.name, null);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (inCutMode)
            {
                SessionEvents.current.CutModeDisabled();
                inCutMode = false;
            }
            else {
                SessionEvents.current.CutModeEnabled();
                inCutMode = true;
            }
        }
    }
}
