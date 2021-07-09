using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
	[Header("Effects")]
	public ParticleSystem trailParticles;
	public GameObject explosionParticles;

	float rotation;

	Rigidbody rb;

	InputManager input;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		input = InputManager.instance;

		trailParticles.Play();
	}

	void Update()
	{
		rotation = input.movementAxis;
	}

	void FixedUpdate()
	{
		Movement(rb, rotation);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<EnemyController>().Catch(collision);
		}
		else 
		{
			GameManager.instance.GameOver();
		}
	}
}
