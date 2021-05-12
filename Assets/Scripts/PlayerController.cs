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
	FieldOfView sensor;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		sensor = GetComponent<FieldOfView>();

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
		if (sensor.visibleTargets.Contains(collision.gameObject.transform))
		{
			collision.gameObject.GetComponent<EnemyController>().Catch(collision);
		}
		else 
		{
			GameManager.instance.GameOver();
		}
	}
}
