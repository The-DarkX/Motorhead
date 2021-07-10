using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationMultiplier = 20f;
    public Transform[] wheels;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float rotationSpeed;

    bool canMove = true;

	private void Awake()
	{
        currentSpeed = moveSpeed;
	}

	virtual public void Movement(Rigidbody rb, float rotation) 
    {
        if (canMove)
        {
            if (GameManager.instance.isGameOn)
            {
                rotationSpeed = currentSpeed * rotationMultiplier;

                rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
                Vector3 yRotation = Vector3.up * rotation * rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(yRotation);
                Quaternion targetRotation = rb.rotation * deltaRotation;
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.fixedDeltaTime));

				foreach (Transform wheel in wheels)
				{
                    wheel.transform.Rotate(currentSpeed, 0, 0);
				}
            }
        }
    }

    virtual public void Stop(Rigidbody rb) 
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
