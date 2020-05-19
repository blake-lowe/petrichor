using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Homebrew;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform cursorPosition;
    public float baseSpeed;
    public float crouchModifier;
    public float sprintModifier;
    public bool isPaused;

    public SpriteRenderer playerSprite;

    [Foldout("Right Hand")] public SpriteRenderer rightHandSpriteRenderer;
    [Foldout("Right Hand")] public Sprite rightHandSpriteSide;
    [Foldout("Right Hand")] public Sprite rightHandSpriteTop;
    [Foldout("Right Hand")] public Vector2 upRelativePosRH;
    [Foldout("Right Hand")] public bool upSortBelowRH = true;
    [Foldout("Right Hand")] public Vector2 downRelativePosRH;
    [Foldout("Right Hand")] public bool downSortBelowRH = false;
    [Foldout("Right Hand")] public Vector2 leftRelativePosRH;
    [Foldout("Right Hand")] public bool leftSortBelowRH = true;
    [Foldout("Right Hand")] public Vector2 rightRelativePosRH;
    [Foldout("Right Hand")] public bool rightSortBelowRH = false;

    [Foldout("Left Hand")] public SpriteRenderer leftHandSpriteRenderer;
    [Foldout("Left Hand")] public Sprite leftHandSpriteSide;
    [Foldout("Left Hand")] public Sprite leftHandSpriteTop;
    [Foldout("Left Hand")] public Vector2 upRelativePosLH;
    [Foldout("Left Hand")] public bool upSortBelowLH = true;
    [Foldout("Left Hand")] public Vector2 downRelativePosLH;
    [Foldout("Left Hand")] public bool downSortBelowLH = false;
    [Foldout("Left Hand")] public Vector2 leftRelativePosLH;
    [Foldout("Left Hand")] public bool leftSortBelowLH = false;
    [Foldout("Left Hand")] public Vector2 rightRelativePosLH;
    [Foldout("Left Hand")] public bool rightSortBelowLH = true;

    private Controls _controls;
    private Vector2 _direction = new Vector2(0, 0);
    private bool _isCrouching;
    private bool _isSprinting;

    private int _stillFramesCount;
    private Vector2 _facing = new Vector2(0, 1);

    private int _playerSpriteSortingOrder;
    private Vector3 _transformPositionRH;
    private Quaternion _transformRotationRH;
    private Vector3 _transformPositionLH;
    private Quaternion _transformRotationLH;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Player.crouch.started += StartCrouch;
        _controls.Player.crouch.canceled += EndCrouch;
        _controls.Player.sprint.started += StartSprint;
        _controls.Player.sprint.canceled += EndSprint;
        _controls.Player.rightHand.performed += HandleRightHand;
        _controls.Player.leftHand.performed += HandleLeftHand;
        _controls.Player.rightLeg.performed += HandleRightLeg;
        _controls.Player.leftLeg.performed += HandleLeftLeg;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _playerSpriteSortingOrder = playerSprite.sortingOrder;
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

    private void HandleRightHand(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Right Hand Action");
        }
    }

    private void HandleLeftHand(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Left Hand Action");
        }
    }

    private void HandleRightLeg(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Right Leg Action");
        }
    }

    private void HandleLeftLeg(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Left Leg Action");
        }
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
        //Calculate Hand Item Positions and Rotations
        var thisPosition = transform.position;
        if (Mathf.Abs(_facing.x) > Mathf.Abs(_facing.y)) //horizontal facing
        {
            if (_facing.x > 0) //facing right
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    rightSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = leftHandSpriteSide;
                rightHandSpriteRenderer.flipX = false;
                _transformPositionRH = new Vector3(rightRelativePosRH.x, rightRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    rightSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteSide;
                leftHandSpriteRenderer.flipX = false;
                _transformPositionLH = new Vector3(rightRelativePosLH.x, rightRelativePosLH.y, 0) + thisPosition;
            }
            else //facing left
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    leftSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = leftHandSpriteSide;
                rightHandSpriteRenderer.flipX = true;
                _transformPositionRH = new Vector3(leftRelativePosRH.x, leftRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    leftSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteSide;
                leftHandSpriteRenderer.flipX = true;
                _transformPositionLH = new Vector3(leftRelativePosLH.x, leftRelativePosLH.y, 0) + thisPosition;
            }
        }
        else //vertical facing
        {
            if (_facing.y > 0) //facing up
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    upSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = rightHandSpriteTop;
                rightHandSpriteRenderer.flipX = false;
                _transformPositionRH = new Vector3(upRelativePosRH.x, upRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    upSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteTop;
                leftHandSpriteRenderer.flipX = true;
                _transformPositionLH = new Vector3(upRelativePosLH.x, upRelativePosLH.y, 0) + thisPosition;
            }
            else //facing down
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    downSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = rightHandSpriteTop;
                rightHandSpriteRenderer.flipX = true;
                _transformPositionRH = new Vector3(downRelativePosRH.x, downRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    downSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteTop;
                leftHandSpriteRenderer.flipX = false;
                _transformPositionLH = new Vector3(downRelativePosLH.x, downRelativePosLH.y, 0) + thisPosition;
            }
        }

        var cursorPos = cursorPosition.transform.position;
        var flipFactorRh = rightHandSpriteRenderer.flipX ? -180 : 0;
        var flipFactorLh = leftHandSpriteRenderer.flipX ? -180 : 0;
        _transformRotationRH = Quaternion.Euler(new Vector3(0, 0,
            Mathf.Atan2(cursorPos.y - _transformPositionRH.y,
                cursorPos.x - _transformPositionRH.x) *
            Mathf.Rad2Deg + flipFactorRh));
        _transformRotationLH = Quaternion.Euler(new Vector3(0, 0,
            Mathf.Atan2(cursorPos.y - _transformPositionLH.y,
                cursorPos.x - _transformPositionLH.x) *
            Mathf.Rad2Deg + flipFactorLh));

        rightHandSpriteRenderer.transform.SetPositionAndRotation(_transformPositionRH, _transformRotationRH);
        leftHandSpriteRenderer.transform.SetPositionAndRotation(_transformPositionLH, _transformRotationLH);
    }
}