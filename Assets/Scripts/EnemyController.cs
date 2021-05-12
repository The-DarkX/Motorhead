using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovementController
{
    public float steerTime = 0.5f;

    [Header("Other")]
    public float scoreIncrement = 20f;
    public float decrementIncrease = 0.1f;
    public ParticleSystem trailParticles;

    private float rotation = 0;

    private GameObject explosionParticles;
    private GameObject catchParticles;

    Rigidbody rb;
    EnemySpawner spawner;
    FieldOfView sensor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawner = GetComponentInParent<EnemySpawner>();
        sensor = GetComponent<FieldOfView>();

        explosionParticles = GameManager.instance.explosionParticles;
        catchParticles = GameManager.instance.catchParticles;

        trailParticles.Play();
    }

	void FixedUpdate()
    {
        Movement(rb, rotation); //Move the vehicle

        if (sensor.visibleTargets.Count > 0) 
        {
			for (int i = 0; i < sensor.visibleTargets.Count; i++)
			{
                if (sensor.visibleTargets[i] != null) 
                {
                    float distanceLeft = Vector3.Distance(-transform.right, sensor.visibleTargets[i].position);
                    float distanceRight = Vector3.Distance(transform.right, sensor.visibleTargets[i].position);

                    if (distanceLeft > distanceRight) //Obstacle on the Right
                    {
                        StartCoroutine(Steer(-1)); //Steer left
                    }
                    else //Obstacle on the Left
                    {
                        StartCoroutine(Steer(1)); //Steer right
                    }
                }
			}
        }
    }

    IEnumerator Steer(float input) 
    {
        rotation = input; // Set rotation
        yield return new WaitForSeconds(steerTime); //steer for a while
        rotation = 0; // Reset rotation
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject != this) // If collides with other objects
        {
            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Ground"))
            {
                DestroyOnContact(collision);
            }
        }
	}

    public void DestroyOnContact(Collision other) 
    {
        trailParticles.Stop();
        Vector3 pos = other.GetContact(0).point;
        GameObject explosion = Instantiate(explosionParticles, pos, explosionParticles.transform.rotation);
        Destroy(explosion, 2);

        AudioManager.instance.PlaySound("Explosion");

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }

    public void Catch(Collision other) 
    {
        trailParticles.Stop();
        Vector3 pos = other.GetContact(0).point;
        GameObject explosion = Instantiate(catchParticles, pos, catchParticles.transform.rotation);
        Destroy(explosion, 2);

        AudioManager.instance.PlaySound("Catch");

        GameManager.instance.AddScore(scoreIncrement);
        GameManager.instance.IncreaseDecrement(decrementIncrease);

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }
}
