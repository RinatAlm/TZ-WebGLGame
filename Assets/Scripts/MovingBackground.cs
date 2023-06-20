using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public float movingSpeed;
    private Vector2 _startPosition;
    private float _repeatWidth;
    private float _repeatPositionX;

    private void Start()
    {
        _startPosition = transform.position;
        _repeatWidth = GetComponent<BoxCollider2D>().size.x/2;
        _repeatPositionX = _startPosition.x - _repeatWidth;
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.gameOver)
        {
            Move();
            if (transform.position.x < _repeatPositionX)//Repeat background
            {
                transform.position = _startPosition;
            }
        }          
    }

    private void Move()
    {
        transform.Translate(movingSpeed * Time.deltaTime * Vector2.left);
    }
}
