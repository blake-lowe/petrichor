using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    //public float speed;
    public Animator animator;
    
    public Pathfinding.AIDestinationSetter aiDestinationSetter;
    public Pathfinding.Patrol patrol;

    public float alertRadius;
    public float timeToNotice;
    
    
    private void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        
    }
}
