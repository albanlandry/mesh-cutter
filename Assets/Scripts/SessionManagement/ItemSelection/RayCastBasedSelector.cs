using UnityEngine;

public class RayCastBasedSelector : MonoBehaviour, ISelector
{
    private Transform selection;
    public void check(Ray ray)
    {
        selection = null;
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            ISelectable selectable = hitPoint.collider.gameObject.GetComponent<ISelectable>();
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
