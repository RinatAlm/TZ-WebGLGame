using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float impulseForse;
    public float liftingForce;
    public float liftTime;
    public Rigidbody2D playerRigidBody;
    private bool onGround;
    private bool stopLifting;
    private Animator playerAnimator;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerRigidBody.velocity.magnitude == 0)
        {
            StopAllCoroutines();
            onGround = true;
            stopLifting = false;
            playerAnimator.SetBool("isOnGround", onGround);
        }

        if (!GameManager.instance.gameOver)
        {
            // Touch theTouch = Input.GetTouch(0);
            if (Input.GetMouseButtonDown(0) && onGround)//Short Jump
            {
                AudioManager.instance.Play("Jump");
                playerRigidBody.AddForce(Vector2.up * impulseForse, ForceMode2D.Impulse);
                StartCoroutine(LiftStopingCoroutine());
                onGround = false;                    
                playerAnimator.SetBool("isOnGround", onGround);
            }
           
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.gameOver)
        {
            if (Input.GetMouseButton(0))//Long jump or continue jumping
            {
                if (playerRigidBody.velocity.y > 0 && !stopLifting)//Add force only if object is lifting
                {
                    playerRigidBody.AddForce(Vector2.up * liftingForce, ForceMode2D.Force);
                }
            }
        }
            
    }

    private IEnumerator LiftStopingCoroutine()
    {
        yield return new WaitForSeconds(liftTime);
        stopLifting = true;
    }
}
