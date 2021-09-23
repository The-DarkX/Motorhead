using System.Collections;
using UnityEngine;
using EZCameraShake;

public class EnemyController : MovementController
{
    public float steerTime = 0.5f;

    [Header("Other")]
    public int minScoreIncrement = 10;
    public int maxScoreIncrement = 30;

    public CollectableType collectableType;
    public bool useCameraShake = true;

    private float rotation = 0;

    private GameObject explosionParticles;
    private GameObject catchParticles;
    private GameObject refueledParticles;

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
        refueledParticles = GameManager.instance.refueledParticles;
    }

	void FixedUpdate()
    {
        if (sensor.visibleTargets.Count > 0)
        {
            int index = sensor.visibleTargets.Count - 1;

            if (sensor.visibleTargets[index] != null)
            {
                currentSpeed = moveSpeed * 0.8f;

                float distanceLeft = Vector3.Distance(-transform.right, sensor.visibleTargets[index].position);
                float distanceRight = Vector3.Distance(transform.right, sensor.visibleTargets[index].position);

                if (distanceLeft > distanceRight) //Obstacle on the Right
                {
                    StartCoroutine(Steer(-1));
                }
                else //Obstacle on the Left
                {
                    StartCoroutine(Steer(1));
                }

                currentSpeed = moveSpeed;
            }
        }

        Movement(rb, rotation);

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
        if (useCameraShake)
            CameraShaker.Instance.ShakeOnce(4, 3, 0, 1);

        trailParticles.Stop();
        Vector3 pos = other.GetContact(0).point;
        GameObject explosion = Instantiate(explosionParticles, pos, explosionParticles.transform.rotation);
        Destroy(explosion, 2);

        AudioManager.instance.PlaySound("Explosion");

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }

    public void ScorePoints(Collision other) 
    {
        if (useCameraShake)
            CameraShaker.Instance.ShakeOnce(5, 3, 0, 1);

        trailParticles.Stop();
        Vector3 pos = other.GetContact(0).point;
        GameObject explosion = Instantiate(catchParticles, pos, catchParticles.transform.rotation);
        Destroy(explosion, 2);

        AudioManager.instance.PlaySound("Catch");

        ScoreSystem.instance.AddScore(Random.Range(minScoreIncrement, maxScoreIncrement));

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }

    public void Refuel(Collision other)
    {
        if (useCameraShake)
            CameraShaker.Instance.ShakeOnce(5, 3, 0, 1);

        trailParticles.Stop();
        Vector3 pos = other.GetContact(0).point;
        GameObject explosion = Instantiate(refueledParticles, pos, refueledParticles.transform.rotation);
        Destroy(explosion, 2);

        AudioManager.instance.PlaySound("Refuel");

        ScoreSystem.instance.AddFuel(Random.Range(minScoreIncrement, maxScoreIncrement));

        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }
}

public enum CollectableType 
{
    Coin,
    Fuel
}