using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController is responsible for handling player input for movement.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private float moveSpeed;

    #endregion

    #region Private Fields

    private CharacterController _characterController;
    private PlayerControls _playerControls;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    #endregion

    #region Initialization Functions

    private void Awake()
    {
        // Get a new instance of CharacterController
        _characterController = GetComponent<CharacterController>();

        // Create a new instance of PlayerControls
        _playerControls = new PlayerControls();

        // Initialize the controls
        InitializeControls();
    }

    private void InitializeControls()
    {
        // Bind the Move action to the OnMove function
        _playerControls.Player.Move.performed += OnMove;
        _playerControls.Player.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        // Enable the player controls when the object is enabled
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        // Disable the player controls when the object is disabled
        _playerControls.Disable();
    }

    #endregion

    #region Input Functions

    private void OnMove(InputAction.CallbackContext obj)
    {
        // Read the value of the input action and store it in _moveInput
        _moveInput = obj.ReadValue<Vector2>();
    }

    #endregion
    
    private void Update()
    {
        // Move the player based on input
        var move = new Vector3(_moveInput.x, 0, _moveInput.y);

        // Convert the move vector to world space
        // _moveDirection = transform.TransformDirection(move);
        _moveDirection = move;

        // Normalize the move direction
        _moveDirection.Normalize();

        // Move the character controller
        _characterController.Move(_moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        // Draw the forward direction of the player
        var forward = transform.forward;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward * 2f);
    }
}