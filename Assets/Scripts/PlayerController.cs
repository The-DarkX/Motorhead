using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
	float rotation;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		rotation = Input.GetAxis("Horizontal");
	}

	void FixedUpdate()
	{
		Movement(rb, rotation);
	}
}
