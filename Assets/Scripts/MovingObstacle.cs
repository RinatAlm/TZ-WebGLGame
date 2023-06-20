using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        if (!GameManager.instance.gameOver)
        {
            Move();
            if (transform.position.x < GameManager.instance.destroyPosX)
                Destroy(gameObject);
        }
           
           
    }

    private void Move()
    {
        transform.Translate( speed * Time.deltaTime * Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))//GameOver if player is inside of collider
        {
            GameManager.instance.GameOver();
        }
       
    }


}
