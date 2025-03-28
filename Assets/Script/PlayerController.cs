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

    // Gestion de l'animation "isJumping" si le personnage est en l'air
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
	
	// private void ApplyRotation()
	// {
	// 	if (_input.sqrMagnitude == 0) return;

	// 	_direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
	// 	var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

	// 	transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	// }

private void ApplyRotation()
{
    // Si l'entrée est nulle, ne pas appliquer de rotation
    if (_input.sqrMagnitude == 0) return; 

    // Calculer la direction du mouvement
    _direction = new Vector3(_input.x, 0.0f, _input.y);

    // Appliquer la rotation basée sur la direction du mouvement
    if (_direction != Vector3.zero)
    {
        // Calculer la rotation cible
        var targetRotation = Quaternion.LookRotation(_direction);

        // Conserver l'orientation initiale à 180°, mais appliquer la direction du mouvement à partir de là
        var rotation = Quaternion.Euler(0f, 180f, 0f) * targetRotation;

        // Appliquer la rotation à une vitesse spécifiée
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