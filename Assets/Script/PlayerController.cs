using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

	private Animator animator;

	#region Variables: Movement

	private Vector2 _input;
	private CharacterController _characterController;
	private Vector3 _direction;

	[SerializeField] private float speed;

	[SerializeField] private Movement movement;

	#endregion
	#region Variables: Rotation

	[SerializeField] private float rotationSpeed = 500f;
	private Camera _mainCamera;

	#endregion
	#region Variables: Gravity

	private float _gravity = -9.81f;
	[SerializeField] private float gravityMultiplier = 3.0f;
	private float _velocity;

	#endregion
	#region Variables: Jumping

	[SerializeField] private float jumpPower;
	private int _numberOfJumps;
	[SerializeField] private int maxNumberOfJumps = 2;

	#endregion
	
	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		if (animator == null)
			{
				Debug.LogError("Animator non trouvé sur l'enfant du Player !");
			}
		_characterController = GetComponent<CharacterController>();
		_mainCamera = Camera.main;

	}

	private void Update()
	{
		ApplyRotation();
		ApplyGravity();
		ApplyMovement();

		
	if (_input.sqrMagnitude > 0.1f || movement.isSprinting)
    {
        animator.SetBool("isMoving", true);
        Debug.Log("--------isMoving = true");
    }
    else
    {
        animator.SetBool("isMoving", false);
        Debug.Log("---------isMoving = false");
    }

    // “isJumping” animation if the character is in the air
    if (!IsGrounded())
    {
        animator.SetBool("isJumping", true);
        Debug.Log("--------isJumping = true");
    }
    else
    {
        animator.SetBool("isJumping", false);
        Debug.Log("------isJumping = false");
    }
	}

	private void ApplyGravity()
	{
		if (IsGrounded() && _velocity < 0.0f)
		{
			_velocity = -1.0f;
		}
		else
		{
			_velocity += _gravity * gravityMultiplier * Time.deltaTime;
		}
		
		_direction.y = _velocity;
	}
	

private void ApplyRotation()
{

    if (_input.sqrMagnitude == 0) return; 


    _direction = new Vector3(_input.x, 0.0f, _input.y);

    if (_direction != Vector3.zero)
    {
        var targetRotation = Quaternion.LookRotation(_direction);

        var rotation = Quaternion.Euler(0f, 180f, 0f) * targetRotation;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}



	private void ApplyMovement()
	{
		var targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
		movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

		_characterController.Move(_direction * movement.currentSpeed * Time.deltaTime);
	}

	public void Move(InputAction.CallbackContext context)
	{
		_input = context.ReadValue<Vector2>();
		_direction = new Vector3(_input.x, 0.0f, _input.y);

	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (!context.started) return;
		if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
		if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());
		
		_numberOfJumps++;
		_velocity = jumpPower;
	}

	public void Sprint(InputAction.CallbackContext context)
	{
		movement.isSprinting = context.started || context.performed;

	}

	private IEnumerator WaitForLanding()
	{
		yield return new WaitUntil(() => !IsGrounded());
		yield return new WaitUntil(IsGrounded);

		_numberOfJumps = 0;
	}

	private bool IsGrounded() => _characterController.isGrounded;



	 public void Respawn()
    {
        _characterController.enabled = false; 
        transform.position = Vector3.zero; 
        transform.rotation = Quaternion.identity; 
        _characterController.enabled = true; 
        _velocity = 0f; 
        movement.currentSpeed = 0f; 
    }
}



[Serializable]
public struct Movement
{
	public float speed;
	public float multiplier;
	public float acceleration;

	[HideInInspector] public bool isSprinting;
	[HideInInspector] public float currentSpeed;
}