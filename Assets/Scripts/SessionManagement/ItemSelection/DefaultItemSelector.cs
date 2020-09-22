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
                if (!selectable.tag.Equals("NOT_SELECTABLE"))
                {
                    if (!selectable.tag.ToLower().Equals("Selectable".ToLower()))
                    {
                        selection = hitPoint.transform;
                    }
                }
            }
        }
    }

    public Transform GetSelection()
    {
        return selection;
    }
}