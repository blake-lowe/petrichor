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

    private Vector3 temp;
    private int _tick;
    private AIDestinationSetter _aiDestinationSetter;
    private float _stopAwareTime;
    private static readonly int KillTriggerID = Animator.StringToHash("kill");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private void Start()
    {
        _noiseSources = FindObjectsOfType<NoiseSource>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetFov(fov);
        facing = Vector2.right;
        temp = Vector3.zero;
        health = startingHealth;
        healthBar.color = healthBarColor;
        healthBar.enabled = false;
        awarenessBar.color = awarenessBarColor;
        awarenessBar.enabled = false;
    }
    private void FixedUpdate()
    {
        if (_tick >= 25) // calls every half a second (FixedUpdate is called every 0.02 seconds)
        {
            _noiseSources = FindObjectsOfType<NoiseSource>();
            _tick = 0;
        }
        _tick++;
    }
    private void Update()
    {
        var currentTime = Time.time;
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

        // enemy listens to noises
        float noiseLevel;
        if (isInvestigatingNoise)
        {
            _aiDestinationSetter.target = null;
        }

        foreach (var noiseSource in _noiseSources)
        {
            noiseLevel = noiseSource.noiseLevel;
            if (Vector3.SqrMagnitude(transform.position - noiseSource.transform.position) <= noiseLevel)
            {
                isInvestigatingNoise = true;
                isPatrolling = false;
                _aiDestinationSetter.target = noiseSource.transform;
                temp = noiseSource.transform.position;
            }
        }
        if (isInvestigatingNoise && Vector3.Distance(transform.position, temp) < 0.2)
        {
            isInvestigatingNoise = false;
            isPatrolling = true;
        }

        //set animator parameters and facing
        if (aiPath.velocity.sqrMagnitude > 0.001)
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
                        isPatrolling = false;
                        isInvestigatingNoise = false;
                        //isAttackingPlayer = true;
                        alertsystem.SoundAlarm();
                        seesPlayer = true;
                        _aiDestinationSetter.target = player.transform; //this is temporary
                        return;
                    }
                }
            }
        }
        seesPlayer = false;
    }

    private void Kill()
    {
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