using UnityEngine;

public class PlayerController : MovementController
{
	float rotation;

	Rigidbody rb;

	InputManager input;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		input = InputManager.instance;
	}

	void Update()
	{
		rotation = input.movementAxis;

		currentSpeed = moveSpeed * (ScoreSystem.instance.fuel / 100);
	}

	void FixedUpdate()
	{
		Movement(rb, rotation);
	}

	private void OnCollisionEnter(Collision collision)
	{
		EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

		if (collision.gameObject.CompareTag("Enemy"))
		{
			switch (enemy.collectableType)
			{
				case CollectableType.Coin:
					enemy.ScorePoints(collision);
					break;

				case CollectableType.Fuel:
					enemy.Refuel(collision);
					break;
			}
		}
		else
		{
			GameManager.instance.GameOver();
		}
	}
}
