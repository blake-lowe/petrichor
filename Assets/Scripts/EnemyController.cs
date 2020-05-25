using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    public FieldOfView fieldOfView;
    public float fov;
    public float viewDistance;
    public GameObject player;

    public Animator animator;
    public ParticleSystem hitPS;
    public AIPath aiPath;
    private NoiseSource[] noiseObjects;
    private Vector2 _facing;
    private bool seesPlayer;

    private void Start()
    {
        //noiseObjects = FindObjectsOfType<NoiseSource>();
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetFov(fov);
        seesPlayer = false;
        noiseObjects = null;
        _facing = Vector2.right;
    }
    private void FixedUpdate()
    {
        int tick = 0;
        if (tick >= 25) // calls every half a second (FixedUpdate is called every 0.02 seconds)
        {
            noiseObjects = FindObjectsOfType<NoiseSource>();
            tick = 0;
        }
        tick++;
    }
    private void Update()
    {
        // enemy listens to noises
        GameObject gameObject;
        float noiseLevel;
        GetComponent<AIDestinationSetter>().target = null;
        if (noiseObjects != null)
        {
            for (int i = 0; i < noiseObjects.Length; i++)
            {
                gameObject = noiseObjects[i].gameObject;
                noiseLevel = gameObject.GetComponent<NoiseSource>().noiseLevel;
                if (Vector3.Distance(transform.position, gameObject.transform.position) <= noiseLevel)
                {
                    GetComponent<AIDestinationSetter>().target = gameObject.transform;
                }
            }
        }


        //set animator parameters and facing
        var xVel = aiPath.velocity.x;
        var yVel = aiPath.velocity.y;
        if (aiPath.velocity.sqrMagnitude > 0.001)
        {
            animator.SetBool("isMoving", true);
            _facing.x = aiPath.velocity.normalized.x;
            _facing.y = aiPath.velocity.normalized.y;
        }else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("Horizontal", _facing.x);
        animator.SetFloat("Vertical", _facing.y);
        
        //set field of view position and rotation
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(new Vector3(_facing.x, _facing.y, 0));
        //correct field of view game object position;
        fieldOfView.transform.position = Vector3.zero;

        FindPlayer();
    }
    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if (Vector3.Angle(_facing, dirToPlayer) <= (fov / 2))
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance);
                if (ray.collider)
                {
                    //raycast hit something
                    if (ray.collider.tag == "Player")
                    {
                        //raycast hit player
                        seesPlayer = true;
                    }
                }
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        WeaponInfo damageInfo = other.GetComponent<WeaponInfo>();
        var damage = 0f;
        if (damageInfo != null)
        {
            damage = damageInfo.damage;
        }
        Debug.Log(damage + " damage taken by " + this.gameObject.name);
        if (hitPS != null)
        {
            var ma = hitPS.main;
            ma.startLifetime = new ParticleSystem.MinMaxCurve(0.07f*damage, 0.2f*damage);
            hitPS.Play();
        }
    }
}
