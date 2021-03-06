﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI_A : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing,
    }
    
    [Header("Movement")]
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float chasingSpeed = 4f;
    [SerializeField] private float rangeOfView = 5f;
    
    [Header("Objects references")]
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private GameObject waypoint;
    
    // position
    private Vector2 startingPosition;
    private Vector2 lastSeenPlayerPosition;
    
    // state machine reference
    private EnemyState enemyState;
    
    void Start()
    {
        enemyState = EnemyState.Idle;

        startingPosition = transform.position;
        waypoint.transform.position = startingPosition;
        
        lastSeenPlayerPosition = Vector2.zero;
    }
    
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                if (Vector2.Distance(transform.position, playerTarget.transform.position) <= rangeOfView)
                    Chase();
                

                enemyState = EnemyState.Chasing;
                break;
            
            case EnemyState.Chasing:
                if (Vector2.Distance(transform.position, playerTarget.transform.position) > rangeOfView)
                    Idle();

                enemyState = EnemyState.Idle;
                break;
        }
    }

    void Idle()
    {
        ChangeFieldOfViewColor(Color.white);
        
        // set target position as a last seen player position, if its not 0 and move to the target
        if (lastSeenPlayerPosition != Vector2.zero)
        {
            //Debug.Log("going to last seen player position");
            waypoint.transform.position = lastSeenPlayerPosition;
            startingPosition = lastSeenPlayerPosition;
            lastSeenPlayerPosition = Vector2.zero; // after going to the last seen player position we gonna continue generating new points from that position, so it has to become 0
        }
        
        FaceTarget(waypoint);
        transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, normalSpeed * Time.deltaTime);

        // change target position within range of view radius
        if (transform.position == waypoint.transform.position)
        {
            float x = Random.Range(startingPosition.x - rangeOfView, startingPosition.x + rangeOfView);
            float y = Random.Range(startingPosition.y - rangeOfView, startingPosition.y + rangeOfView);
            
            waypoint.transform.position = new Vector2(x,y);
        }
    }

    void Chase()
    {
        if (Vector2.Distance(transform.position, playerTarget.transform.position) > 0.5f)
        {
            ChangeFieldOfViewColor(Color.red);
            
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

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            lastSeenPlayerPosition = new Vector2(col.transform.position.x, col.transform.position.y);
            Debug.Log("Last seen player position: " + lastSeenPlayerPosition);
        }
    }
    
}
