using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform cursorPosition;
    public float baseSpeed;
    public float crouchModifier;
    public float sprintModifier;

    private Controls _controls;
    private Vector2 _direction = new Vector2(0,0);
    private bool _isCrouching;
    private bool _isSprinting;

    private int _stillFramesCount;
    private Vector2 _facing = new Vector2(0, 1);

    private void Awake()
    {
        _controls = new Controls();
        
        _controls.Player.crouch.started += StartCrouch;
        _controls.Player.crouch.canceled += EndCrouch;
        _controls.Player.sprint.started += StartSprint;
        _controls.Player.sprint.canceled += EndSprint;
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
        //Move Player
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
        //Set Animator Parameters
        _facing = (cursorPosition.position - transform.position).normalized;
        animator.SetFloat("FacingX", _facing.x);
        animator.SetFloat("FacingY", _facing.y);
        animator.SetBool("isSprinting", _isSprinting);
        animator.SetBool("isCrouching", _isCrouching);
        if (_direction.sqrMagnitude < 0.005)
        {
            _stillFramesCount++;
        }
        else
        {
            _stillFramesCount = 0;
        }
        animator.SetBool("isMoving", _stillFramesCount < 3);
        animator.SetBool("isReversing", Vector2.Dot(_direction, _facing) < 0);
    }
}
