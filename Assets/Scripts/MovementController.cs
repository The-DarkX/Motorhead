using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    virtual public void Movement(Rigidbody rb, float rotation) 
    {
        rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
        Vector3 yRotation = Vector3.up * rotation * rotationSpeed * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(yRotation);
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime));
    }
}