using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6.0f;
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // We move forward by z and rigth by x
        Vector3 move = transform.forward * z + transform.right * x;

        // Move the player by the newly computed offset
        controller.Move(move * speed * Time.deltaTime);
    }
}
