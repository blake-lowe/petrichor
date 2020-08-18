using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    public float startingHealth;
    public float health;
    public float maxAwareness = 3;
    public float awareness;
    public float awareDuration = 1;
    public float hearing = 1;
    public SpriteRenderer healthBar;
    public Color healthBarColor;
    public SpriteRenderer awarenessBar;
    public Color awarenessBarColor;
    public Color awareOfPlayerColor;
    public Color stunnedColor;
    public FieldOfView fieldOfView;
    public float fov;
    public float viewDistance;
    public LayerMask raycastLayerMask;
    public GameObject player;
    public Collider2D collider2d;
    public Animator animator;
    public ParticleSystem hitPS;
    public AIPath aiPath;
    private NoiseSource[] _noiseSources;
    private NoiseSource _noiseSourceToInvestigate;
    private Transform _noiseSourceToInvestigateTransform;
    public bool _hasReachedNoiseSource = true;
    public Vector2 facing;
    public bool seesPlayer;
    public bool awareOfPlayer;

    public bool isPatrolling = true;
    public bool isInvestigatingNoise = false;
    public bool isAttackingPlayer = false;

    private bool _isDead;
    public bool isStunned;
    private float _timeToStopStun;
    
    
    public AlertSystem alertsystem;
    
    private int _tick;
    private AIDestinationSetter _aiDestinationSetter;
    private Patrol _patrol;
    private float _stopAwareTime;
    private static readonly int KillTriggerID = Animator.StringToHash("kill");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private void Start()
    {
        _noiseSources = FindObjectsOfType<NoiseSource>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _patrol = GetComponent<Patrol>();
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetFov(fov);
        facing = Vector2.right;
        health = startingHealth;
        healthBar.color = healthBarColor;
        healthBar.enabled = false;
        awarenessBar.color = awarenessBarColor;
        awarenessBar.enabled = false;
    }

    private void Patrol()
    {
        isPatrolling = true;
        isInvestigatingNoise = false;
        isAttackingPlayer = false;
        
        _patrol.enabled = true;
        _aiDestinationSetter.enabled = false;
    }

    private void InvestigateNoise()
    {
        if (_noiseSourceToInvestigate)
        {
            isPatrolling = false;
            isInvestigatingNoise = true;
            isAttackingPlayer = false;
        
            _patrol.enabled = false;
            _aiDestinationSetter.target = _noiseSourceToInvestigateTransform;
            _aiDestinationSetter.enabled = true;
        }
    }

    private void AttackPlayer()
    {
        isPatrolling = false;
        isInvestigatingNoise = false;
        isAttackingPlayer = true;
        _patrol.enabled = false;
        _aiDestinationSetter.target = player.transform;
        _aiDestinationSetter.enabled = true;
        if (awareOfPlayer && !isStunned)//check for los and not stunned
        {
            //shoot gun if rof time passed
            //reset shot timer for next shot
        }
    }

    private void StopAttackPlayer()
    {
        isAttackingPlayer = false;
        if (_noiseSourceToInvestigate)
        {
            InvestigateNoise();
        }
        else
        {
            Patrol();
        }
    }
    
    
    private void FixedUpdate()
    {
        if (_tick >= 25) // calls every half a second (FixedUpdate is called every 0.02 seconds) to mitigate expensive call
        {
            _noiseSources = FindObjectsOfType<NoiseSource>();
            _tick = 0;
        }
        _tick++;
    }
    private void Update()
    {
        var currentTime = Time.time;
        
        //set behavior state
        if (_noiseSourceToInvestigate && Vector3.SqrMagnitude(_noiseSourceToInvestigate.transform.position - transform.position) < 1f)
        {
            _hasReachedNoiseSource = true;
            _noiseSourceToInvestigate = null;
            //TODO do a 360
        }

        if (!awareOfPlayer)
        {
            StopAttackPlayer();
        }
        
        if (awareOfPlayer)
        {
            AttackPlayer();
        }
        else if (_noiseSourceToInvestigate)
        {
            InvestigateNoise();
        }
        else if (!_isDead && _hasReachedNoiseSource)
        {
            Patrol();
        }
        
        
        
        if (health <= 0)
        {
            Kill();
        }

        if (currentTime > _timeToStopStun)
        {
            isStunned = false;
        }

        if (!_isDead)
        {
            aiPath.enabled = !isStunned;
        }

        // enemy listens to noises and picks the loudest one accounting for distance falloff (inverse square)
        foreach (var noiseSource in _noiseSources)
        {
            var sqrDist = Vector3.SqrMagnitude(transform.position - noiseSource.transform.position);
            if ((noiseSource.noiseLevel * hearing / sqrDist)  > 1f)
            {
                if (_noiseSourceToInvestigate)
                {
                    var noiseSourceToInvestigateVolume = _noiseSourceToInvestigate.noiseLevel / 
                                                            Vector3.SqrMagnitude(transform.position - _noiseSourceToInvestigate.transform.position);
                    if (noiseSource.noiseLevel / sqrDist > noiseSourceToInvestigateVolume)
                    {
                        _noiseSourceToInvestigate = noiseSource;
                        _hasReachedNoiseSource = false;
                    }
                }
                else
                {
                    _noiseSourceToInvestigate = noiseSource;
                    _hasReachedNoiseSource = false;
                }
                _noiseSourceToInvestigateTransform = _noiseSourceToInvestigate.transform;

            }
        }

        //set animator parameters and facing
        if (aiPath.velocity.sqrMagnitude > 0.001f)
        {
            animator.SetBool(IsMoving, true);
            facing.x = aiPath.velocity.normalized.x;
            facing.y = aiPath.velocity.normalized.y;
        }else
        {
            animator.SetBool(IsMoving, false);
        }
        animator.SetFloat(Horizontal, facing.x);
        animator.SetFloat(Vertical, facing.y);
        
        //set field of view position and rotation
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(new Vector3(facing.x, facing.y, 0));
        //correct field of view game object position;
        fieldOfView.transform.position = Vector3.zero;

        FindPlayer();
        
        //calculate awareness
        if (seesPlayer && !isStunned)
        {
            awareness += Time.deltaTime;
        }
        else if (currentTime > _stopAwareTime)
        {
            awareness -= Time.deltaTime;
            awareOfPlayer = false;
        }

        if (awareness > maxAwareness)
        {
            awareness = maxAwareness;
            _stopAwareTime = currentTime + awareDuration;
            awareOfPlayer = true;
        }

        if (awareness < 0)
        {
            awareness = 0;
        }

        //set awareness bar color
        awarenessBar.color = awareOfPlayer ? awareOfPlayerColor : awarenessBarColor;
        if (isStunned)
        {
            awarenessBar.color = stunnedColor;
        }
        
        //set awareness bar
        if (awareness > 0 && health > 0)
        {
            awarenessBar.enabled = true;
            var percentage = awareness / maxAwareness;
            var t = awarenessBar.transform;
            var scale = t.localScale;
            t.localScale = new Vector3(16f * percentage, scale.y, scale.z);
            var pos = t.position;
            t.position = new Vector3(-0.25f * percentage +transform.position.x, pos.y, pos.z);
        }
    }
    
    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if (Vector3.Angle(facing, dirToPlayer) <= (fov / 2))
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, raycastLayerMask);
                if (ray.collider)
                {
                    //raycast hit something
                    if (ray.collider.tag == "Player")
                    {
                        //raycast hit player

                        //isAttackingPlayer = true;
                        //alertsystem.SoundAlarm();
                        seesPlayer = true;
                        return;
                    }
                }
            }
        }
        seesPlayer = false;
    }

    public void Kill()
    {
        health = 0;
        _isDead = true;
        animator.SetTrigger(KillTriggerID);
        aiPath.enabled = false;
        collider2d.enabled = false;
        fieldOfView.gameObject.SetActive(false);
        healthBar.enabled = false;
        awarenessBar.enabled = false;
    }

    public void Stun(float time)
    {
        isStunned = true;
        _timeToStopStun = Time.time + time;
    }
    
    private void OnParticleCollision(GameObject other)
    {
        //get collisionEvent
        
        var weaponInfo = other.GetComponent<WeaponInfo>();
        var ps = other.GetComponent<ParticleSystem>();
        var collisionEvents = weaponInfo.collisionEvents;
        other.GetComponent<ParticleSystem>().GetCollisionEvents(gameObject, collisionEvents);
        
        foreach (var collisionEvent in collisionEvents)
        {
            //handle collision effects
            var damage = 0f;
            if (weaponInfo != null)
            {
                damage = weaponInfo.damage;
            }
            health -= damage;
            if (health < startingHealth)//update health bar
            {
                healthBar.enabled = true;
                var percentage = health / startingHealth;
                var t = healthBar.transform;
                var scale = t.localScale;
                t.localScale = new Vector3(16f * percentage, scale.y, scale.z);
                var pos = t.position;
                t.position = new Vector3(-0.25f * percentage +transform.position.x, pos.y, pos.z);
            }
            if (hitPS != null)
            {
                var position = collisionEvent.intersection;
                var direction = collisionEvent.velocity.normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                hitPS.transform.position = position;
                hitPS.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, angle));
                //var ma = hitPS.main;
                //ma.startLifetime = new ParticleSystem.MinMaxCurve(0.07f*damage, 0.2f*damage);
                hitPS.Play();
            }
        }
    }
}