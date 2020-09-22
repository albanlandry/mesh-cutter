using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScriptManager : MonoBehaviour
{
    [SerializeField] LookAround lookAround;
    // Start is called before the first frame update
    void Start()
    {
        SessionEvents.current.OnCutModeEnable += EnableScript;
        SessionEvents.current.OnCutModeDisable += DisableScript;
        SessionEvents.current.OnSelectionEnable += EnableScript;
        SessionEvents.current.OnSelectionDisable += DisableScript;
    }

    private void OnDisable()
    {
        SessionEvents.current.OnCutModeEnable -= EnableScript;
        SessionEvents.current.OnCutModeDisable -= DisableScript;
        SessionEvents.current.OnSelectionEnable -= EnableScript;
        SessionEvents.current.OnSelectionDisable -= DisableScript;
    }

    void EnableScript()
    {
        lookAround.enabled = false;
    }

    void DisableScript()
    {
        lookAround.enabled = true;
    }
}
