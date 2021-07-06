using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls controls;

	public float movementAxis;

	public Vector3 acceleration;

	public bool pauseButton = false;

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

		controls.Gameplay.Pause.performed += Jump;
	}


	void Jump(InputAction.CallbackContext context) 
	{
		pauseButton = !pauseButton;
		Debug.Log(pauseButton);
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
