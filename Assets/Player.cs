using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Animator animator;

    [Header("Move Info")]
    [SerializeField] float moveSpeed;
    [SerializeField] float singleJumpForce;
    [SerializeField] float doubleJumpForce;

    [Header("Raycast Info")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask layerMask;

    bool playerUnlocked;
    bool isGrounded;
    bool canDoubleJump;

    void Start()
    {
        // Rigidbody2D 컴포넌트 참조
        myRigidbody = GetComponent<Rigidbody2D>();
        // Animator 컴포넌트 참조
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorController();

        CheckCollision();

        CheckInput();

        if (playerUnlocked)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }

    }

    private void AnimatorController()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetFloat("xVelocity", myRigidbody.velocity.x);
        animator.SetFloat("yVelocity", myRigidbody.velocity.y);
    }

    private void CheckCollision()
    {
        // 레이캐스트
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, layerMask);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            playerUnlocked = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, singleJumpForce);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, doubleJumpForce);
            canDoubleJump = false;
        }
    }

    private void OnDrawGizmos()
    {
        // 레이캐스트 렌더링
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
