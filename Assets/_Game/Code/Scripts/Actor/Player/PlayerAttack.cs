using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField, Required] private CinemachineBrain cinemachineBrain;
    [SerializeField, Required] private Transform firePoint;
    [SerializeField, Required] private PlayerProjectile projectilePrefab;

    [SerializeField] private float rotationSpeed = 100f;
    
    [SerializeField] private RangedValue attackCooldown;

    [SerializeField] private UnityEvent<PlayerAttack> onAttack;

    #endregion

    #region Private Fields

    private PlayerControls _playerControls;

    private Vector2 _lookInput;
    private Vector3 _lookPosition;
    private bool _isAttacking;

    #endregion

    #region Initialization Functions

    private void Awake()
    {
        // Create a new instance of PlayerControls
        _playerControls = new PlayerControls();

        // playerWeapons.Add(new DebugPlayerWeapon());

        // Initialize the controls
        InitializeControls();
    }

    private void InitializeControls()
    {
        // Bind the Look action to the OnLook function
        _playerControls.Player.Look.performed += OnLook;
        _playerControls.Player.Look.canceled += OnLook;

        // Bind the Attack action to the OnAttack function
        _playerControls.Player.Attack.performed += OnAttack;
        _playerControls.Player.Attack.canceled += OnAttack;
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

    #region

    /// <summary>
    /// This method assumes the player is using the mouse to look around.
    /// </summary>
    /// <param name="obj"></param>
    private void OnLook(InputAction.CallbackContext obj)
    {
        // Get the look input
        _lookInput = obj.ReadValue<Vector2>();
    }


    private void OnAttack(InputAction.CallbackContext obj)
    {
        _isAttacking = obj.ReadValue<float>() > 0;
    }

    #endregion

    private void Update()
    {
        // Update the look position based on the mouse input
        UpdateLookPosition();

        // Update the rotation based on the look position
        UpdateRotation();

        // Update the attack cooldown
        attackCooldown.ChangeValue(-Time.deltaTime);

        // Update the attack
        UpdateAttack();
    }

    /// <summary>
    /// Get the point in the world where the mouse is pointing in relation to the camera
    /// </summary>
    private void UpdateLookPosition()
    {
        var mainCam = cinemachineBrain.OutputCamera;

        // Get the ray from the camera to the mouse position
        var cameraRay = mainCam.ScreenPointToRay(_lookInput);

        var plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(cameraRay, out var distance))
            _lookPosition = cameraRay.GetPoint(distance);

        // Make sure the y position is the same as the player's y position
        _lookPosition.y = transform.position.y;
    }

    private void UpdateRotation()
    {
        // Set the rotation based on the look position
        var lookRotation = Quaternion.LookRotation(_lookPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateAttack()
    {
        // Return if not attacking
        if (!_isAttacking)
            return;

        // Return if the attack cooldown is > 0
        if (attackCooldown.CurrentValue > 0)
            return;
        
        // Reset the attack cooldown
        attackCooldown.ChangeValue(attackCooldown.MaxValue);
        
        // Instantiate the projectile
        var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        
        // Call the shoot method on the projectile
        projectile.Shoot(this, firePoint.position, firePoint.forward);
    }

    private void OnDrawGizmos()
    {
        // Draw the point where the mouse is pointing in the world
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_lookPosition, .5f);
    }
}