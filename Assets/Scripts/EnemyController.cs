using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovementController
{
    public float steerTime = 0.5f;

    [Header("Effects")]
    public ParticleSystem trailParticles;
    public GameObject explosionParticles;

    [Header("Other")]
    public float scoreIncrement = 20f;
    public float decrementIncrease = 0.1f;

    private float rotation;
    Rigidbody rb;
    EnemySpawner spawner;
    FieldOfView sensor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawner = GetComponentInParent<EnemySpawner>();
        sensor = GetComponent<FieldOfView>();

        trailParticles.Play();
    }

	void FixedUpdate()
    {
        Movement(rb, rotation); //Move the vehicle

        if (sensor.visibleTargets.Count > 0) 
        {
            float distanceLeft = Vector3.Distance(-transform.right, sensor.visibleTargets[0].position);
            float distanceRight = Vector3.Distance(transform.right, sensor.visibleTargets[0].position);

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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Vector3 pos = collision.GetContact(0).point;
                if (Physics.OverlapSphere(pos, 5, 1).Length != 0)
                {
                    GameObject explosion = Instantiate(explosionParticles, pos, Quaternion.identity);
                    Destroy(explosion, 15f);
                }

                DestroyOnContact();
            }
        }
	}

    public void DestroyOnContact() 
    {
        trailParticles.Stop();
        AudioManager.instance.PlaySound("Explosion");

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }

    public void Catch() 
    {
        trailParticles.Stop();

        AudioManager.instance.PlaySound("Explosion");

        GameManager.instance.AddScore(scoreIncrement);
        GameManager.instance.IncreaseDecrement(decrementIncrease);

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }
}
