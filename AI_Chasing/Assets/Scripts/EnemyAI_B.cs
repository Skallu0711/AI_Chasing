using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_B : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing,
    }

    public float normalSpeed = 2f;
    public float chasingSpeed = 4f;
    public float rangeOfView = 5f;
    
    private Waypoints waypoints;
    private int waypointIndex;
    
    public GameObject playerTarget;
    
    public Vector2 lastPosition;
    
    private EnemyState enemyState;
    
    public bool isChasing = false;

    void Start()
    {
        enemyState = EnemyState.Idle;

        waypoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();

        lastPosition = transform.position;
    }
    
    void Update()
    { 
        switch (enemyState)
        {
            case EnemyState.Idle:
                if (Vector2.Distance(transform.position, playerTarget.transform.position) <= rangeOfView)
                {
                    isChasing = true;
                    Chase();
                }

                enemyState = EnemyState.Chasing;
                break;
            
            case EnemyState.Chasing:
                if (Vector2.Distance(transform.position, playerTarget.transform.position) > rangeOfView)
                {
                    isChasing = false;
                    
                    // return to the last position
                    Vector2 direction = new Vector2(lastPosition.x - transform.position.x, lastPosition.y - transform.position.y);
                    transform.up = direction;
                    
                    transform.position = Vector2.MoveTowards(transform.position, lastPosition, normalSpeed * Time.deltaTime);
                    
                    // start idling again
                    if (Vector2.Distance(transform.position, lastPosition) < 0.1f)
                        Idle();
                }

                enemyState = EnemyState.Idle;
                break;
        }
    }

    // point to point patrolling
    void Idle()
    {
        ChangeFieldOfViewColor(Color.white);
        
        lastPosition = transform.position;

        FaceTarget(waypoints.waypoints[waypointIndex]);
        transform.position = Vector2.MoveTowards(transform.position, waypoints.waypoints[waypointIndex].transform.position, normalSpeed * 1.5f * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints.waypoints[waypointIndex].transform.position) < 0.1f)
        {
            if (waypointIndex < waypoints.waypoints.Length - 1)
                waypointIndex++;
            else
                waypointIndex = 0;
        }
    }

    void Chase()
    {
        if (Vector2.Distance(transform.position, playerTarget.transform.position) > 0.5)
        {
            ChangeFieldOfViewColor(Color.blue);

            FaceTarget(playerTarget);
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, chasingSpeed * Time.deltaTime);
        }
    }
    
    // rotate towards target
    void FaceTarget(GameObject target)
    {
        Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        transform.up = direction;
    }
    
    // field of view is a child of the enemy object, so this is how we change it's color
    void ChangeFieldOfViewColor(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.material.color = color;
    }

}
