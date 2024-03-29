using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationMultiplier = 20f;
    public Transform[] wheels;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float rotationSpeed;

    [Header("Effects")]
    public ParticleSystem trailParticles;

    bool canMove = true;

    bool spinWheels;

	private void Awake()
	{
        currentSpeed = moveSpeed;
	}

	private void Start()
	{
        if (wheels == null) 
            spinWheels = false;
        else 
            spinWheels = true;
	}

	virtual public void Movement(Rigidbody rb, float rotation) 
    {
        if (canMove)
        {
            if (FindObjectOfType<GameManager>() == null)
                return;

            else if (FindObjectOfType<GameManager>() != null && GameManager.instance.isGameOn)
            {
                //trailParticles.Play();
                //trailParticles.enableEmission = true;

                rotationSpeed = currentSpeed * rotationMultiplier;

                rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
                Vector3 yRotation = Vector3.up * rotation * rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(yRotation);
                Quaternion targetRotation = rb.rotation * deltaRotation;
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.fixedDeltaTime));

                if (spinWheels) return;

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
