using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultItemSelector : MonoBehaviour, ISelector
{
    private Transform selection;
    public void check(Ray ray)
    {
        selection = null;
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            GameObject selectable = hitPoint.collider.gameObject;
            if (selectable != null)
            {
                selection = hitPoint.transform;
            }
        }
    }

    public Transform GetSelection()
    {
        return selection;
    }
}