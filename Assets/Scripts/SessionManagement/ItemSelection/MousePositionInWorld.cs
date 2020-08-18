using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionInWorld : MonoBehaviour, IMousePosition
{
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
