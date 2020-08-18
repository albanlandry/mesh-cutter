using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModel : MonoBehaviour
{
    private void OnMouseUp()
    {
        SessionEvents.current.MeshModelClick(gameObject);
    }
}
