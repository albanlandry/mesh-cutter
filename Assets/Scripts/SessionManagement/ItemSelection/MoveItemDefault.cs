using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemDefault : MonoBehaviour, IMoveItem
{
    public void MoveItem(GameObject obj, Vector3 newPosition)
    {
        obj.transform.position = newPosition;
    }
}
