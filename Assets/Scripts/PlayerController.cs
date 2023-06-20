using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float impulseForse;
    public float liftingForce;
    private Rigidbody2D playerRigidBody;
    private bool onGround;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerRigidBody.velocity.magnitude == 0)
        {
            onGround = true;
        }

        if (!GameManager.instance.gameOver)
        {
            // Touch theTouch = Input.GetTouch(0);
            if (Input.GetMouseButtonDown(0) && onGround)//Short Jump
            {
                playerRigidBody.AddForce(Vector2.up * impulseForse, ForceMode2D.Impulse);
                onGround = false;
            }
            else if (Input.GetMouseButton(0))//Long jump or continue jumping
            {
                playerRigidBody.AddForce(Vector2.up * liftingForce, ForceMode2D.Force);
            }
        }
    }
}
