using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    private Rigidbody rb;
    private bool launched = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!launched) {
            rb.isKinematic = false;

            // Debug.Log("x: " + Mathf.Abs(transform.eulerAngles.x) + "y: " + Mathf.Abs(transform.eulerAngles.y) + "z: " + Mathf.Abs(transform.eulerAngles.z));
            rb.AddForce(transform.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
            launched = true;

            // Detach the children from the parent
            transform.parent = null;
        }
    }
}
