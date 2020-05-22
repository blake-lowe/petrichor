using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using Homebrew;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform cursorPosition;
    public float baseSpeed;
    public float crouchModifier;
    public float sprintModifier;
    public bool isPaused;
    public float crouchingNoise;
    public float walkingNoise;
    public float sprintingNoise;
    

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
    [Foldout("Right Hand")] public ParticleSystem rightHandBullets;
    [Foldout("Right Hand")] public WeaponInfo rightHandWeaponInfo;
    [Foldout("Right Hand")] public TextMeshProUGUI rightHandAmmoField;
    //[Foldout("Right Hand")] public Vector2 rightHandBulletsSidePos;
    //[Foldout("Right Hand")] public Vector2 rightHandBulletsTopPos;
    
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
    [Foldout("Left Hand")] public ParticleSystem leftHandBullets;
    [Foldout("Left Hand")] public WeaponInfo leftHandWeaponInfo;
    [Foldout("Left Hand")] public TextMeshProUGUI leftHandAmmoField;
    //[Foldout("Left Hand")] public Vector2 leftHandBulletsSidePos;
    //[Foldout("Left Hand")] public Vector2 leftHandBulletsTopPos;
    
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
    
    private float _noiseLevel;
    private float _nextFireTimeLh;
    private float _nextFireTimeRh;
    private bool _isFiringLh;
    private bool _isFiringRh;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Player.crouch.started += StartCrouch;
        _controls.Player.crouch.canceled += EndCrouch;
        _controls.Player.sprint.started += StartSprint;
        _controls.Player.sprint.canceled += EndSprint;
        _controls.Player.rightHand.started += HandleRightHandStart;
        _controls.Player.leftHand.started += HandleLeftHandStart;
        _controls.Player.rightLeg.started += HandleRightLegStart;
        _controls.Player.leftLeg.started += HandleLeftLegStart;
        _controls.Player.rightHand.canceled += HandleRightHandCancel;
        _controls.Player.leftHand.canceled += HandleLeftHandCancel;
        _controls.Player.rightLeg.canceled += HandleRightLegCancel;
        _controls.Player.leftLeg.canceled += HandleLeftLegCancel;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _playerSpriteSortingOrder = playerSprite.sortingOrder;
        leftHandAmmoField.text = string.Format("%d/%d", leftHandWeaponInfo.currentBullets, leftHandWeaponInfo.totalBullets);
        rightHandAmmoField.text = string.Format("%d/%d", rightHandWeaponInfo.currentBullets, rightHandWeaponInfo.totalBullets);
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

    private void HandleRightHandStart(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            if (rightHandBullets != null)
            {
                if (Time.time > _nextFireTimeRh)
                {
                    if (rightHandWeaponInfo.currentBullets > 0)
                    {
                        rightHandBullets.Play();
                        rightHandWeaponInfo.ReduceCurrentBullets(1);
                        _nextFireTimeRh = Time.time + 1 / rightHandWeaponInfo.fireRate;
                        if (rightHandWeaponInfo.isFullAuto)
                        {
                            _isFiringRh = true;
                        }
                    }
                    else
                    {
                        _isFiringRh = false;
                    }
                    
                }
            }
        }
    }

    private void HandleLeftHandStart(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            if (leftHandBullets != null)
            {
                if (Time.time > _nextFireTimeLh)
                {
                    if (leftHandWeaponInfo.currentBullets > 0)
                    {
                        leftHandBullets.Play();
                        leftHandWeaponInfo.ReduceCurrentBullets(1);
                        _nextFireTimeLh = Time.time + 1 / leftHandWeaponInfo.fireRate;
                        if (leftHandWeaponInfo.isFullAuto)
                        {
                            _isFiringLh = true;
                        }
                    }
                    else
                    {
                        _isFiringLh = false;
                    }
                    
                }
            }
        }
    }
    
    private void HandleRightHandCancel(InputAction.CallbackContext context)
    {
        if (rightHandBullets != null)
        {
            //rightHandBullets.Stop();
            _isFiringRh = false;
        }
    }

    private void HandleLeftHandCancel(InputAction.CallbackContext context)
    {
        if (leftHandBullets != null)
        {
            //leftHandBullets.Stop();
            _isFiringLh = false;
        }
    }

    private void HandleRightLegStart(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Right Leg Action Start");
        }
    }

    private void HandleLeftLegStart(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Debug.Log("Left Leg Action Start");
        }
    }
    private void HandleRightLegCancel(InputAction.CallbackContext context)
    {
        
    }

    private void HandleLeftLegCancel(InputAction.CallbackContext context)
    {
        
    }

    private void FixedUpdate()
    {
        var currentTime = Time.time;
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
                rightHandSpriteRenderer.sprite = rightHandSpriteSide;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRH = new Vector3(rightRelativePosRH.x, rightRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    rightSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteSide;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, -1, 1);
                _transformPositionLH = new Vector3(rightRelativePosLH.x, rightRelativePosLH.y, 0) + thisPosition;
            }
            else //facing left
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    leftSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = rightHandSpriteSide;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, -1, 1);
                _transformPositionRH = new Vector3(leftRelativePosRH.x, leftRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    leftSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteSide;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLH = new Vector3(leftRelativePosLH.x, leftRelativePosLH.y, 0) + thisPosition;
            }
            //Bullet emitter positions
            //rightHandBullets.gameObject.transform.position = new Vector3(rightHandBulletsSidePos.x, 
                //rightHandBulletsSidePos.y, 0) + _transformPositionRH;
            //leftHandBullets.gameObject.transform.position = new Vector3(leftHandBulletsSidePos.x, 
                //leftHandBulletsSidePos.y, 0) + _transformPositionLH;
        }
        else //vertical facing
        {
            if (_facing.y > 0) //facing up
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    upSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = rightHandSpriteTop;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRH = new Vector3(upRelativePosRH.x, upRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    upSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteTop;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLH = new Vector3(upRelativePosLH.x, upRelativePosLH.y, 0) + thisPosition;
            }
            else //facing down
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    downSortBelowRH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = rightHandSpriteTop;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRH = new Vector3(downRelativePosRH.x, downRelativePosRH.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    downSortBelowLH ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = leftHandSpriteTop;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLH = new Vector3(downRelativePosLH.x, downRelativePosLH.y, 0) + thisPosition;
            }
            //Bullet emitter positions
            //rightHandBullets.gameObject.transform.position = new Vector3(rightHandBulletsTopPos.x,
                //rightHandBulletsSidePos.y, 0) + _transformPositionRH;
            //leftHandBullets.gameObject.transform.position = new Vector3(leftHandBulletsTopPos.x, 
                //leftHandBulletsSidePos.y, 0) + _transformPositionLH;
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
        
        //handle firing of full auto weapons, subtracting of ammunition
        //left hand
        if (_isFiringLh & currentTime > _nextFireTimeLh)
        {
            if (leftHandWeaponInfo.currentBullets > 0)
            {
                leftHandBullets.Play();
                leftHandWeaponInfo.ReduceCurrentBullets(1);
                _nextFireTimeLh = currentTime + 1 / leftHandWeaponInfo.fireRate;
            }
            else
            {
                _isFiringLh = false;
            }
           
        }
        //right hand
        if (_isFiringRh & currentTime > _nextFireTimeRh)
        {
            if (rightHandWeaponInfo.currentBullets > 0)
            {
                rightHandBullets.Play();
                rightHandWeaponInfo.ReduceCurrentBullets(1);
                _nextFireTimeRh = currentTime + 1 / rightHandWeaponInfo.fireRate;
            }
            else
            {
                _isFiringRh = false;
            }
            
        }
        //update ui of ammo levels
        leftHandAmmoField.text = $"{leftHandWeaponInfo.currentBullets}/{leftHandWeaponInfo.totalBullets}";
        rightHandAmmoField.text = $"{rightHandWeaponInfo.currentBullets}/{rightHandWeaponInfo.totalBullets}";
        
        
        //set noise level of player based on crouching/walking/sprinting status
        if (_direction == new Vector2(0,0))
        {
            _noiseLevel = 0;
        }
        else
        {
            if (_isCrouching)
            {
                _noiseLevel = crouchingNoise;
            }
            else if (_isSprinting)
            {
                _noiseLevel = sprintingNoise;
            }
            else
            {
                _noiseLevel = walkingNoise;
            }
        }

        GetComponent<NoiseSource>().noiseLevel = _noiseLevel;
    }
}