using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    // movement directions
    private float horizontalMovement;
    private float verticalMovement;
    
    // x and y coordinates
    private float x;
    private float y;

    void Update()
    {
        Move();
    }

    void Move()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        x = transform.position.x + speed * Time.deltaTime * horizontalMovement;
        
        verticalMovement = Input.GetAxisRaw("Vertical");
        y = transform.position.y + speed * Time.deltaTime * verticalMovement;
        
        transform.position = new Vector2(x,y);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("player dead");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
}
