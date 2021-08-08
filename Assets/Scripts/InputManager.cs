using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    PlayerControls controls;

	public float movementAxis;

	public Vector3 acceleration;

	public float minSwipeDistance = 0.5f;

	public static InputManager instance { get; private set; }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}

		controls = new PlayerControls();

		controls.Gameplay.Movement.performed += ctx => movementAxis = ctx.ReadValue<float>();
		controls.Gameplay.Movement.canceled += _ => movementAxis = 0;

		//controls.Gameplay.Touch.started += _=>print("pressed");
		//controls.Gameplay.Touch.canceled += _ => print("released");
	}

	private void OnEnable()
	{
		controls.Gameplay.Enable();
	}

	private void OnDisable()
	{
		controls.Gameplay.Disable();
	}
}
