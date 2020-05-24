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
    private GameObject[] noiseObjects;
    private Vector2 _facing;
    private bool seesPlayer;

    private void Start()
    {
        noiseObjects = GameObject.FindGameObjectsWithTag("NoiseObject");
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetFov(fov);
        seesPlayer = false;
        _facing = Vector2.right;
    }
    
    private void Update()
    {
        GameObject gameObject;
        Vector2 vectorDifference;
        float noiseLevel;
        GetComponent<AIDestinationSetter>().target = null;
        for (int i = 0; i < noiseObjects.Length; i++)
        {
            gameObject = noiseObjects[i];
            Vector2 enemyVector = new Vector2(transform.position.x, transform.position.y);
            Vector2 objectVector = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            vectorDifference = enemyVector - objectVector;
            noiseLevel = gameObject.GetComponent<NoiseSource>().noiseLevel;

            if (vectorDifference.magnitude < noiseLevel)
            {
                GetComponent<AIDestinationSetter>().target = gameObject.transform;
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
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance);
                if (raycast.collider)
                {
                    //raycast hit something
                    if (PrefabUtility.GetCorrespondingObjectFromSource(raycast.collider.gameObject) == PrefabUtility.GetCorrespondingObjectFromSource(player))
                    {
                        //raycast hit player
                        seesPlayer = true;
                        Debug.Log("how is this happening");
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
