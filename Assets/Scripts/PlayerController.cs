using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;

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
    public float interactRadiusSqr;
    public float stealthKillRadiusSqr;
    private float _nextStealthKillTime;
    public float stealthKillDuration;
    public bool isFrozen;
    public GameStateController gameStateController;
    public NoiseSource playerNoiseSource;
    public NoiseSource leftHandNoiseSource;

    public SpriteRenderer playerSprite;
    public GameObject groundItemPrefab;
    public Transform groundItemsParent;
    
    private bool _isRightHandFull = false;
    public SpriteRenderer rightHandSpriteRenderer;
    public Image rightHandSpriteRendererUi;
    public TextMeshProUGUI rightHandNameField;
    public Sword rightHandSword;
    public Animator rightHandBlade;

    public List<Gadget> gadgets;
    private int _gadgetIndex;
    public Image gadgetSpriteRendererUi;
    private GameObject _currentGadgetPrefab;
    private GadgetController _activeGadgetController;
    public GameObject cancelUtilityButton;
    public TextMeshProUGUI gadgetNameField;
    public LineRenderer gadgetTrajectoryRenderer;
    private Vector3 _gadgetTarget;
    public int gadgetPhase = 0;
    
    
    private Sprite _rightHandSpriteSide;
    private Sprite _rightHandSpriteTop;

    public Vector2 upRelativePosRh;
    public bool upSortBelowRh = true;
    public Vector2 downRelativePosRh;
    public bool downSortBelowRh = false;
    public Vector2 leftRelativePosRh;
    public bool leftSortBelowRh = true;
    public Vector2 rightRelativePosRh;
    public bool rightSortBelowRh = false;

    private bool _isLeftHandFull = false;
    public SpriteRenderer leftHandSpriteRenderer;
    public Image leftHandSpriteRendererUi;
    public GameObject leftHandParticleSystemParent;
    public TextMeshProUGUI leftHandNameField;
    public TextMeshProUGUI leftHandAmmoField;
    public Weapon leftHandWeapon;
    
    private Sprite _leftHandSpriteSide;
    private Sprite _leftHandSpriteTop;
    private ParticleSystem _leftHandBullets;
    private WeaponInfo _leftHandWeaponInfo;
    public AmmoCounter leftHandAmmoCounter;
    
    public Vector2 upRelativePosLh;
    public bool upSortBelowLh = true;
    public Vector2 downRelativePosLh;
    public bool downSortBelowLh = false;
    public Vector2 leftRelativePosLh;
    public bool leftSortBelowLh = false;
    public Vector2 rightRelativePosLh;
    public bool rightSortBelowLh = true;

    public bool hasDashAugment;
    public float dashDistance;
    public float dashSpeed;
    private bool _isDashing;
    private Vector3 _dashTarget;
    public EchoEffect dashEchoEffect;

    public bool hasShieldAugment;
    public bool shieldEnabled;
    
    public bool hasCannonAugment;
    
    
    
    private Controls _controls;
    private Vector2 _direction = new Vector2(0, 0);
    private bool _isCrouching;
    private bool _isSprinting;
    public bool isDead = false;

    private int _stillFramesCount;
    private Vector2 _facing = new Vector2(0, 1);

    private int _playerSpriteSortingOrder;
    private Vector3 _transformPositionRh;
    private Quaternion _transformRotationRh;
    private Vector3 _transformPositionLh;
    private Quaternion _transformRotationLh;
    
    private float _noiseLevel;
    private float _nextFireTimeLh;
    private float _nextSwingTimeRh;
    private bool _isFiringLh;

    private Weapon _weaponToSwap;
    private AmmoCounter _weaponToSwapAmmoCounter;
    private GameObject _weaponToSwapGameObject;

    private Sword _swordToSwap;
    private GameObject _swordToSwapGameObject;
    
    private float _stopNoiseTimeLh;
    private float _weaponNoiseDuration;
    
    private static readonly int FacingX = Animator.StringToHash("FacingX");
    private static readonly int FacingY = Animator.StringToHash("FacingY");
    private static readonly int IsSprinting = Animator.StringToHash("isSprinting");
    private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsReversing = Animator.StringToHash("isReversing");

    private void Start()
    {
        _controls = new Controls();

        _controls.Player.crouch.started += StartCrouch;
        _controls.Player.crouch.canceled += EndCrouch;
        _controls.Player.sprint.started += StartSprint;
        _controls.Player.sprint.canceled += EndSprint;
        _controls.Player.rightHand.started += HandleRightHandStart;
        _controls.Player.leftHand.started += HandleLeftHandStart;
        _controls.Player.rightHand.canceled += HandleRightHandCancel;
        _controls.Player.leftHand.canceled += HandleLeftHandCancel;
        _controls.Player.interact.performed += HandleInteract;
        _controls.Player.utility.started += HandleUtility;
        _controls.Player.directionalDash.performed += HandleDashAugment;
        _controls.Player.augment1.started += HandleCannonAugment;
        
        _controls.Player.Enable();
        
        _playerSpriteSortingOrder = playerSprite.sortingOrder;
        leftHandAmmoField.text = _isLeftHandFull ? 
            $"{leftHandAmmoCounter.currentAmmo}/{leftHandAmmoCounter.totalAmmo}" : "";
        if (leftHandWeapon)
        {
            InitializeAmmoCounter(leftHandWeapon.weaponPrefab.GetComponent<WeaponInfo>(), "left");
            EquipWeapon(leftHandWeapon, leftHandAmmoCounter);
        }

        if (rightHandSword)
        {
            EquipSword(rightHandSword);
        }

        if (gadgets.Count > 0)
        {
            EquipGadget(0);
        }

        shieldEnabled = hasShieldAugment;
    }

    private void InitializeAmmoCounter(WeaponInfo original, string hand)
    {
        if (hand.Equals("left"))
        {
            leftHandAmmoCounter.currentAmmo = original.currentBullets;
            leftHandAmmoCounter.totalAmmo = original.totalBullets;
        }
        else if (hand.Equals("right"))
        {
            Debug.Log("No ranged weapons in right hand");
        }
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
        // TODO Add melee attack + unarmed attack
        //play animation
        rightHandBlade.SetTrigger("Swing");
        //kill enemies in range
        float numCasts = 24;
        var enemiesInRange = new List<EnemyController>();
        var startAngle = rightHandBlade.gameObject.transform.rotation.eulerAngles.z - (rightHandSword.arc / 2f);
        for (float i = 0; i < numCasts; i++)
        {
            var angle = (startAngle + (i / numCasts) * rightHandSword.arc) * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            var raycastHit = Physics2D.Raycast(rightHandBlade.transform.position, direction, rightHandSword.reach);
            if (raycastHit)
            {
                EnemyController enemy = raycastHit.collider.gameObject.GetComponent<EnemyController>();
                if (enemy && !enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                }
            }
            
        }

        foreach (var enemyController in enemiesInRange)
        {
            enemyController.Kill();
        }
        //start parry state
        
    }

    private void HandleLeftHandStart(InputAction.CallbackContext context)
    {
        if (!isPaused && !isFrozen && !isDead)
        {
            if (leftHandWeapon)
            {
                if (Time.time > _nextFireTimeLh)
                {
                    if (leftHandAmmoCounter.currentAmmo > 0)
                    {
                        _leftHandBullets.Play();
                        leftHandAmmoCounter.currentAmmo--;
                        _nextFireTimeLh = Time.time + 1 / _leftHandWeaponInfo.fireRate;
                        if (_leftHandWeaponInfo.isFullAuto)
                        {
                            _isFiringLh = true;
                        }
                        leftHandNoiseSource.noiseLevel = _leftHandWeaponInfo.noiseLevel;
                        _stopNoiseTimeLh = Time.time + _leftHandWeaponInfo.noiseDuration;
                    }
                    else
                    {
                        _isFiringLh = false;
                    }
                    
                }
            }
            else
            {
                if (Time.time > _nextStealthKillTime)
                {
                    UnarmedAttack();
                }
            }
        }
        
    }

    private void UnarmedAttack()
    {
        EnemyController closestEnemy;
        _nextStealthKillTime = Time.time + stealthKillDuration;
        EnemyController[] enemyControllers = FindObjectsOfType<EnemyController>();
        if (enemyControllers.Length > 0)
        {
            var minDistSqr = Mathf.Infinity;
            closestEnemy = enemyControllers[0];
            foreach (var enemyController in enemyControllers)
            {
                var distSqr = (transform.position - enemyController.transform.position).sqrMagnitude;
                if (distSqr < minDistSqr)
                {
                    minDistSqr = distSqr;
                    closestEnemy = enemyController;
                }
            }

            if (minDistSqr > stealthKillRadiusSqr)
            {
                return;
            }
        }
        else
        {
            return;
        }
                    
        if (!closestEnemy.seesPlayer)//check enemy sight
        {
            //flip enemy so that the death anim plays more realistically.
            /*
            if (closestEnemy.facing.x > closestEnemy.facing.y)
            {
                closestEnemy.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                closestEnemy.transform.localScale = new Vector3(1, -1, 1);
            }
            */
            closestEnemy.health = 0;//kill enemy
            isFrozen = true; //freeze player
        }
    }

    private void HandleRightHandCancel(InputAction.CallbackContext context)
    {
        //TODO stop parry state
    }

    private void HandleLeftHandCancel(InputAction.CallbackContext context)
    {
        if (_leftHandBullets)
        {
            _isFiringLh = false;
        }
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (!isPaused && !isFrozen && !isDead)
        {
            //find closest interactable object
            GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
            GameObject closest = interactables[0];
            var minDistance = Mathf.Infinity;
            foreach (GameObject interactable in interactables)
            {
                var thisDistance = (interactable.transform.position - transform.position).sqrMagnitude;
                if (thisDistance < minDistance)
                {
                    minDistance = thisDistance;
                    closest = interactable;
                }
            }

            if (minDistance > interactRadiusSqr)
            {
                return;
            } //if object too far away

            //handle based on attached script
            var gir = closest.GetComponent<GroundItemRenderer>();
            if (gir) //dropped weapon
            {
                if (gir.weapon)
                {
                    if (!_isLeftHandFull)
                    {
                        EquipWeapon(gir.weapon, gir.ammoCounter);
                        Destroy(closest);
                    }
                    else
                    {
                        _weaponToSwap = gir.weapon;
                        _weaponToSwapAmmoCounter = gir.ammoCounter;
                        _weaponToSwapGameObject = closest;
                        gameStateController.swapWeaponPanel.SetActive(true);
                        gameStateController.PauseGame();
                    }
                }
                else if (gir.sword)
                {
                    if (!_isRightHandFull)
                    {
                        EquipSword(gir.sword);
                        Destroy(closest);
                    }
                    else
                    {
                        _swordToSwap = gir.sword;
                        _swordToSwapGameObject = closest;
                        gameStateController.swapSwordPanel.SetActive(true);
                        gameStateController.PauseGame();
                    }
                }
            }
        }
    }

    private void HandleUtility(InputAction.CallbackContext context)
    {
        if (!isPaused && !isDead && !isFrozen && gadgets.Count > 0)
        {
            if (gadgets[_gadgetIndex].deployable)
            {
                switch (gadgetPhase)
                {
                    case 0://ready->primed
                        cancelUtilityButton.SetActive(true);
                        gadgetTrajectoryRenderer.enabled = true;
                        //activate highlight
                        gadgetPhase = 1;
                        break;
                    case 1://primed->thrown
                        cancelUtilityButton.SetActive(false);
                        gadgetTrajectoryRenderer.enabled = false;
                        //change highlight color to red or something
                        //instantiate prefab
                        _activeGadgetController = Instantiate(_currentGadgetPrefab).GetComponent<GadgetController>();
                        _activeGadgetController.playerController = this;
                        _activeGadgetController.transform.position = transform.position;
                        _activeGadgetController.targetPos = _gadgetTarget;
                        gadgetPhase = 2;
                        break;
                    case 2://thrown->used
                        _activeGadgetController.ActivateAbility();
                        //remove the gadget from inventory
                        gadgets.Remove(gadgets[_gadgetIndex]);
                        if (_gadgetIndex >= gadgets.Count)
                        {
                            _gadgetIndex = 0;
                        }
                        EquipGadget(_gadgetIndex);
                        gadgetPhase = 0;
                        break;
                }
            }
            else
            {
                _activeGadgetController = Instantiate(_currentGadgetPrefab).GetComponent<GadgetController>();
                _activeGadgetController.playerController = this;
                _activeGadgetController.transform.position = transform.position;
                _activeGadgetController.ActivateAbility();
                gadgets.Remove(gadgets[_gadgetIndex]);
                if (_gadgetIndex >= gadgets.Count)
                {
                    _gadgetIndex = 0;
                }
                EquipGadget(_gadgetIndex);
                gadgetPhase = 0;
            }
        }
    }

    private void HandleDashAugment(InputAction.CallbackContext context)
    {
        if (hasDashAugment && !isPaused && !isFrozen && !isDead)
        {
            var colliderRadius = 0.5f;
            var distance = dashDistance;
            var raycastHit = Physics2D.Raycast(transform.position, _direction, dashDistance + colliderRadius);
            if (raycastHit)
            {
                distance = raycastHit.distance - colliderRadius;
            }
            //move player
            isFrozen = true;
            _isDashing = true;
            _dashTarget = transform.position + distance * new Vector3(_direction.x, _direction.y, 0).normalized;
            //create visual effect
            dashEchoEffect.enabled = true;
        }
        
    }

    private void HandleCannonAugment(InputAction.CallbackContext context)
    {
        if (hasCannonAugment && !isPaused && !isFrozen && !isDead)
        {
            Debug.Log("Cannon Augment");
        }
    }

        public void CancelUtility()
    {
        if (gadgetPhase == 1)
        {
            gadgetPhase = 0;
            cancelUtilityButton.SetActive(false);
            gadgetTrajectoryRenderer.enabled = false;
        }
    }

    public void DropRight()
    {
        if (_isRightHandFull)
        {
            var tempSword = rightHandSword;
            Unequip("right");
            var groundItem = Instantiate(groundItemPrefab, groundItemsParent);
            var gir = groundItem.GetComponent<GroundItemRenderer>();
            gir.sword = tempSword;
            groundItem.transform.position = transform.position;
        }
    }

    public void DropLeft()
    {
        if (_isLeftHandFull)
        {
            var tempWeapon = leftHandWeapon;
            var tempAmmoCounter = leftHandAmmoCounter;
            Unequip("left");
            var groundItem = Instantiate(groundItemPrefab, groundItemsParent);
            var gir = groundItem.GetComponent<GroundItemRenderer>();
            gir.weapon = tempWeapon;
            gir.ammoCounter = tempAmmoCounter;
            groundItem.transform.position = transform.position;
        }
    }

    public void SwapRight()
    {
        if (_isRightHandFull)
        {
            DropRight();
        }
        EquipSword(_swordToSwap);
        Destroy(_weaponToSwapGameObject);
        gameStateController.swapSwordPanel.SetActive(false);
    }

    public void SwapLeft()
    {
        if (_isLeftHandFull)
        {
            DropLeft();
        }
        EquipWeapon(_weaponToSwap, _weaponToSwapAmmoCounter);
        Destroy(_weaponToSwapGameObject);
        gameStateController.swapWeaponPanel.SetActive(false);
    }
    

    private void Unequip(string hand)//hand = "left"/"right"
    {
        if (hand.Equals("left"))
        {
            leftHandWeapon = null;
            _leftHandBullets = null;
            foreach (Transform child in leftHandParticleSystemParent.transform)
            {
                Destroy(child.gameObject);
            }
            _leftHandSpriteSide = null;
            _leftHandSpriteTop = null;
            leftHandSpriteRenderer.sprite = null;
            _leftHandWeaponInfo = null;
            leftHandSpriteRendererUi.sprite = null;
            leftHandSpriteRendererUi.color = new Color(1, 1, 1, 0);//make the placeholder sprite transparent
            leftHandNameField.text = "empty";
            leftHandAmmoField.text = "";
            _isLeftHandFull = false;
        }
        else if (hand.Equals("right"))
        {
            rightHandSword = null;
            rightHandSpriteRenderer.sprite = null;
            _rightHandSpriteSide = null;
            _rightHandSpriteTop = null;
            rightHandSpriteRendererUi.sprite = null;
            rightHandSpriteRendererUi.color = new Color(1, 1, 1, 0);//make the placeholder sprite transparent
            rightHandNameField.text = "empty";
            _isRightHandFull = false;
        }
    }
    private void EquipWeapon(Weapon weapon, AmmoCounter ammoCounter)
    {
        leftHandWeapon = weapon;
        var leftHandParticleSystemChild = Instantiate(weapon.weaponPrefab, leftHandParticleSystemParent.transform);
        _leftHandBullets = leftHandParticleSystemChild.GetComponent<ParticleSystem>();
        _leftHandSpriteSide = weapon.spriteSide;
        _leftHandSpriteTop = weapon.spriteTop;
        _leftHandWeaponInfo = leftHandParticleSystemChild.GetComponent<WeaponInfo>();
        leftHandAmmoCounter = ammoCounter;
        leftHandSpriteRendererUi.sprite = weapon.spriteUi;
        leftHandSpriteRendererUi.color = new Color(1, 1, 1, 1);
        leftHandNameField.text = weapon.name;
        leftHandAmmoField.text = $"{leftHandAmmoCounter.currentAmmo}/{leftHandAmmoCounter.totalAmmo}";
        _isLeftHandFull = true;
    }

    private void EquipSword(Sword sword)
    {
        rightHandSword = sword;
        _rightHandSpriteSide = sword.spriteSide;
        _rightHandSpriteTop = sword.spriteTop;
        rightHandSpriteRendererUi.sprite = sword.spriteUi;
        rightHandSpriteRendererUi.color = new Color(1, 1, 1, 1);
        rightHandNameField.text = sword.name;
        _isRightHandFull = true;
    }

    private void EquipGadget(int gadgetIndexToLoad)
    {
        if (gadgetIndexToLoad < gadgets.Count)
        {
            _gadgetIndex = gadgetIndexToLoad;
            gadgetSpriteRendererUi.sprite = gadgets[gadgetIndexToLoad].spriteUi;
            _currentGadgetPrefab = gadgets[gadgetIndexToLoad].gadgetPrefab;
            gadgetNameField.text = gadgets[gadgetIndexToLoad].name;
        }

        if (gadgets.Count == 0)
        {
            _currentGadgetPrefab = null;
            gadgetNameField.text = "Empty";
        }
    }

    public void GadgetUp()
    {
        var newIndex = _gadgetIndex + 1;
        if (newIndex >= gadgets.Count)
        {
            newIndex = 0;
        }
        EquipGadget(newIndex);
    }

    public void GadgetDown()
    {
        var newIndex = _gadgetIndex - 1;
        if (newIndex < 0)
        {
            newIndex = gadgets.Count - 1;
        }
        EquipGadget(newIndex);
    }

    private void FixedUpdate()
    {
        var currentTime = Time.time;
        //Move Player
        _direction = isFrozen ? Vector2.zero : _controls.Player.direction.ReadValue<Vector2>();
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

        if (currentTime > _nextStealthKillTime & !_isDashing)
        {
            isFrozen = false;
        }

        if (_isDashing)
        {
            if ((_dashTarget - transform.position).magnitude < dashSpeed)
            {
                transform.position = _dashTarget;
            }
            else
            {
                transform.position += (_dashTarget - transform.position).normalized * dashSpeed;
            }

            if ((_dashTarget - transform.position).sqrMagnitude < 0.001f)
            {
                isFrozen = false;
                _isDashing = false;
                dashEchoEffect.enabled = false;
            }
        }
        
        //set opacity of gadget renderer
        gadgetSpriteRendererUi.color = gadgets.Count > 0 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //update positions of gadget targeter
        var source = new Vector2(transform.position.x, transform.position.y);
        var cursorDirection = new Vector2(cursorPosition.position.x, cursorPosition.position.y) - source;
        var targetPosition = Physics2D.Raycast(source, cursorDirection).point;
        if ((targetPosition - source).sqrMagnitude > (cursorDirection).sqrMagnitude)
        {
            targetPosition = cursorPosition.position;
        }
        else
        {
            targetPosition -= cursorDirection.normalized * 0.2f;
        }
        gadgetTrajectoryRenderer.SetPositions(new []{transform.position, new Vector3(targetPosition.x, targetPosition.y, 0), });
        _gadgetTarget = targetPosition;

        //Set Animator Parameters
        if (!isFrozen)
        {
            _facing = (cursorPosition.position - transform.position).normalized;
            animator.SetFloat(FacingX, _facing.x);
            animator.SetFloat(FacingY, _facing.y);
            animator.SetBool(IsSprinting, _isSprinting);
            animator.SetBool(IsCrouching, _isCrouching);
            if (_direction.sqrMagnitude < 0.005)
            {
                _stillFramesCount++;
            }
            else
            {
                _stillFramesCount = 0;
            }

            animator.SetBool(IsMoving, _stillFramesCount < 3);
            animator.SetBool(IsReversing, Vector2.Dot(_direction, _facing) < 0);
        }
        else
        {
            animator.SetBool(IsMoving, false);
        }
            
        //Calculate Hand Item Positions and Rotations
        var thisPosition = transform.position;
        if (Mathf.Abs(_facing.x) > Mathf.Abs(_facing.y)) //horizontal facing
        {
            //correct particle emitters for handle length
            if (_isLeftHandFull)
            {
                leftHandParticleSystemParent.transform.position = 
                    leftHandSpriteRenderer.transform.position + new Vector3(0, leftHandWeapon.handleLength, 0);
            }
            if (_facing.x > 0) //facing right
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    rightSortBelowRh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = _rightHandSpriteSide;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                rightHandBlade.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRh = new Vector3(rightRelativePosRh.x, rightRelativePosRh.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    rightSortBelowLh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = _leftHandSpriteSide;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, -1, 1);
                _transformPositionLh = new Vector3(rightRelativePosLh.x, rightRelativePosLh.y, 0) + thisPosition;
            }
            else //facing left
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    leftSortBelowRh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = _rightHandSpriteSide;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                rightHandBlade.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                _transformPositionRh = new Vector3(leftRelativePosRh.x, leftRelativePosRh.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    leftSortBelowLh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = _leftHandSpriteSide;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLh = new Vector3(leftRelativePosLh.x, leftRelativePosLh.y, 0) + thisPosition;
            }
        }
        else //vertical facing
        {
            //correct particle emitters for handle length
            leftHandParticleSystemParent.transform.position = leftHandSpriteRenderer.transform.position;
            if (_facing.y > 0) //facing up
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    upSortBelowRh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = _rightHandSpriteTop;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                rightHandBlade.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRh = new Vector3(upRelativePosRh.x, upRelativePosRh.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    upSortBelowLh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = _leftHandSpriteTop;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLh = new Vector3(upRelativePosLh.x, upRelativePosLh.y, 0) + thisPosition;
            }
            else //facing down
            {
                //Right hand
                rightHandSpriteRenderer.sortingOrder =
                    downSortBelowRh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                rightHandSpriteRenderer.sprite = _rightHandSpriteTop;
                rightHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                rightHandBlade.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionRh = new Vector3(downRelativePosRh.x, downRelativePosRh.y, 0) + thisPosition;
                //Left hand
                leftHandSpriteRenderer.sortingOrder =
                    downSortBelowLh ? _playerSpriteSortingOrder - 1 : _playerSpriteSortingOrder + 1;
                leftHandSpriteRenderer.sprite = _leftHandSpriteTop;
                leftHandSpriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
                _transformPositionLh = new Vector3(downRelativePosLh.x, downRelativePosLh.y, 0) + thisPosition;
            }
        }

        var cursorPos = cursorPosition.transform.position;
        var flipFactorRh = rightHandSpriteRenderer.flipX ? -180 : 0;
        var flipFactorLh = leftHandSpriteRenderer.flipX ? -180 : 0;
        
        _transformRotationRh = Quaternion.Euler(new Vector3(0, 0,
            Mathf.Atan2(cursorPos.y - _transformPositionRh.y,
                cursorPos.x - _transformPositionRh.x) *
           Mathf.Rad2Deg + flipFactorRh));
        
        _transformRotationLh = Quaternion.Euler(new Vector3(0, 0,
            Mathf.Atan2(cursorPos.y - _transformPositionLh.y,
                cursorPos.x - _transformPositionLh.x) *
            Mathf.Rad2Deg + flipFactorLh));
        rightHandBlade.gameObject.transform.rotation = _transformRotationRh;
        if (!isFrozen)
        {
            rightHandSpriteRenderer.transform.SetPositionAndRotation(_transformPositionRh, quaternion.identity);
            leftHandSpriteRenderer.transform.SetPositionAndRotation(_transformPositionLh, _transformRotationLh);
        }
        //only display weapons hands are full
        leftHandSpriteRenderer.gameObject.SetActive(_isLeftHandFull);
        rightHandSpriteRenderer.gameObject.SetActive(_isRightHandFull);
        
        //handle firing of full auto weapons, subtracting of ammunition
        //left hand
        if (_isFiringLh & currentTime > _nextFireTimeLh)
        {
            if (leftHandAmmoCounter.currentAmmo > 0)
            {
                _leftHandBullets.Play();
                leftHandAmmoCounter.currentAmmo--;
                _nextFireTimeLh = currentTime + 1 / _leftHandWeaponInfo.fireRate;
                leftHandNoiseSource.noiseLevel = _leftHandWeaponInfo.noiseLevel;
                _stopNoiseTimeLh = Time.time + _leftHandWeaponInfo.noiseDuration;
            }
            else
            {
                _isFiringLh = false;
            }
           
        }
        //stop weapon noise sources if enough time has passed
        if (currentTime > _stopNoiseTimeLh)
        {
            leftHandNoiseSource.noiseLevel = 0;
        }
        
        //update ui of ammo levels
        leftHandAmmoField.text = _isLeftHandFull ? 
            $"{leftHandAmmoCounter.currentAmmo}/{leftHandAmmoCounter.totalAmmo}" : "";

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

        playerNoiseSource.noiseLevel = _noiseLevel;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (shieldEnabled)
        {
            shieldEnabled = false;
        }
        else
        {
            //player death
            isDead = true;
            gameStateController.Respawn();
        }
    }
}