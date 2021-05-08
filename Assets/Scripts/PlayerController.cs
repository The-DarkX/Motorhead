using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
	[Header("Effects")]
	public ParticleSystem trailParticles;

	float rotation;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		trailParticles.Play();
	}

	void Update()
	{
		rotation = Input.GetAxis("Horizontal");
	}

	void FixedUpdate()
	{
		Movement(rb, rotation);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<EnemyController>().Catch();
		}
	}
}
