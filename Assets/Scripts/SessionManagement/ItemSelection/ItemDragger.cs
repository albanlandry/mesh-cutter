using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragger : MonoBehaviour
{
    /*
    private IRayProvider rayProvider;
    */
    private Rigidbody rb;
    private Vector3 offset = Vector3.zero;
    private Vector3 prevMousePos = Vector3.zero; // Keep track of the previous mouse position on click
    IMoveItem moveAction; // Implements the moving action of the item
    IMousePosition mousePosition; // Helps in getting the mouse position

    private void Awake()
    {
        // Initialize the script by retrieving them from the ones attached to the game object
        moveAction = GetComponent<IMoveItem>();
        mousePosition = GetComponent<IMousePosition>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        // Initialize the script by retrieving them from the ones attached to the game object
        moveAction = GetComponent<IMoveItem>();
        mousePosition = GetComponent<IMousePosition>();
        
        rb.isKinematic = true;
        offset = gameObject.transform.position - mousePosition.GetMousePosition();
        prevMousePos = mousePosition.GetMousePosition();
    }

    private void OnMouseDrag()
    {
        Debug.Log("Mouse drawing");
        // Compute the item z-coordinate relative to its current position
        Vector3 curMousePos = mousePosition.GetMousePosition();

        // If the the mouse is going upward, we increase the depth.
        // Otherwise if it is negative we just decrease it.
        // In case it is 0 we do not modify the depth, it means there was no mouse motion
        /*
        float yDiff = curMousePos.y - prevMousePos.y;
        // Debug.Log(yDiff);
        if (yDiff > 0) {
            curMousePos.z += yDiff;
        } else if (yDiff < 0) {
            curMousePos.z -= yDiff;
        }

        curMousePos.z = curMousePos.z;
        */


        moveAction.MoveItem(gameObject, curMousePos + offset);
        prevMousePos = curMousePos;// Quit track of the current position for the next iteration.

    }

    private void OnMouseUp()
    {
        rb.isKinematic = false;
    }
}
