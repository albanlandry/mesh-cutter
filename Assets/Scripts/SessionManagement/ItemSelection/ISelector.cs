using UnityEngine;

public interface ISelector
{
    void check(Ray ray);
    Transform GetSelection();
}
