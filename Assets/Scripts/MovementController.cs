using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    float currentSpeed;

    bool canMove = true;

	private void Awake()
	{
        SetSpeed(moveSpeed);
	}

	virtual public void Movement(Rigidbody rb, float rotation) 
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.deltaTime);
            Vector3 yRotation = Vector3.up * rotation * rotationSpeed * Time.deltaTime;
            Quaternion deltaRotation = Quaternion.Euler(yRotation);
            Quaternion targetRotation = rb.rotation * deltaRotation;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime));
        }
    }

    virtual public void Stop(Rigidbody rb) 
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    virtual public void SetSpeed(float newSpeed) 
    {
        currentSpeed = newSpeed;
    }
}
