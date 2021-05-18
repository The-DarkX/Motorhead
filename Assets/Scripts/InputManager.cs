using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls controls;

	public static float movementAxis;

	public static Vector3 acceleration;

	private void Awake()
	{
		controls = new PlayerControls();

		if (Keyboard.current != null)
		{
			controls.Gameplay.Movement.performed += ctx => movementAxis = ctx.ReadValue<float>();
			controls.Gameplay.Movement.canceled += _ => movementAxis = 0;
		}
	}

	private void Update()
	{
		if (Accelerometer.current != null)
		{
			acceleration = Accelerometer.current.acceleration.ReadValue();
			print(acceleration);
		}
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
