using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableItemSelector : MonoBehaviour, ISelector
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
                if(selectable.gameObject.tag.Equals("Selectable")
                    || (selectable.transform.parent != null && selectable.transform.parent.tag.Equals("Selectable")))
                {
                    selection = hitPoint.transform;
                }
            }
        }
    }

    public Transform GetSelection()
    {
        return selection;
    }
}
