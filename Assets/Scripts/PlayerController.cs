using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float baseSpeed;
    public float crouchModifier;
    public float sprintModifier;

    private Controls _controls;
    private Vector2 _direction = new Vector2(0,0);
    private bool _isCrouching;
    private bool _isSprinting;

    private void Awake()
    {
        _controls = new Controls();
        
        _controls.Player.crouch.started += StartCrouch;
        _controls.Player.crouch.canceled += EndCrouch;
        _controls.Player.sprint.started += StartSprint;
        _controls.Player.sprint.canceled += EndSprint;
        //_controls.Player.direction.started += HandleMove;
        //_controls.Player.direction.canceled += HandleMove;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    private void StartCrouch(InputAction.CallbackContext context)
    {
        _isSprinting = false;
        _isCrouching = true;
    }

    private void EndCrouch(InputAction.CallbackContext context)
    {
        _isCrouching = false;
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        _isCrouching = false;
        _isSprinting = true;
    }

    private void EndSprint(InputAction.CallbackContext context)
    {
        _isSprinting = false;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        _direction = _controls.Player.direction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _direction = _controls.Player.direction.ReadValue<Vector2>();
        var movement = _direction * baseSpeed;
        if (_isCrouching)
        {
            movement *= crouchModifier;
        }

        if (_isSprinting)
        {
            movement *= sprintModifier;
        }
        rb.AddForce(new Vector2(movement.x, movement.y), ForceMode2D.Force);
    }
}
