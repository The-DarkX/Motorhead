using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovementController
{
    public float steerTime = 0.5f;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Transform[] sensorPositions;
    public LayerMask hitMask;

    [Header("Other")]
    public float scoreIncrement = 20f;
    public float decrementIncrease = 0.1f;
    public bool showIndicators = false;

    private RaycastHit[] hits;
    private float rotation;
    Rigidbody rb;
    EnemySpawner spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawner = GetComponentInParent<EnemySpawner>();

        hits = new RaycastHit[sensorPositions.Length]; //Generating hits array
    }

	void FixedUpdate()
    {
        Sensors(); //Check the sensors
        Movement(rb, rotation); //Move the vehicle
    }

    void Sensors() 
    {
		for (int i = 0; i < sensorPositions.Length; i++)
		{
            Vector3 direction = sensorPositions[i].forward.normalized; // Creates laser direction

            if (Physics.Raycast(transform.position, direction, out hits[i], sensorLength, hitMask)) //If object detected on lasers
            {
                if (showIndicators)
                    Debug.DrawRay(sensorPositions[i].position, direction * sensorLength, Color.red); //Draw laser representation (found)

                if (sensorPositions[i].localPosition.x > 0) //Sensor on the Right
                {
                    StartCoroutine(Steer(-1)); //Steer left
                }
                else if (sensorPositions[i].localPosition.x < 0) //Sensor on the Left
                {
                    StartCoroutine(Steer(1));//Steer right
                }
            }
            else
            {
                if (showIndicators)
                    Debug.DrawRay(sensorPositions[i].position, direction * sensorLength, Color.green); //Draw laser representation
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
        if (collision.gameObject != this && !collision.gameObject.CompareTag("Ground")) // If collides with other objects
        {
            if (collision.gameObject.CompareTag("Player")) 
            {
                GameManager.instance.AddScore(scoreIncrement);
                GameManager.instance.IncreaseDecrement(decrementIncrease);
            }

            DestroyOnContact();
        }
	}

    void DestroyOnContact() 
    {
        spawner.enemies.Remove(this); // Removed from enemies list
        Destroy(gameObject);
    }
}