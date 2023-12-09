using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Raycast Info")]
    public float groundCheckDistance;
    public LayerMask layerMask;

    private bool isRunning;
    private bool isGrounded;

    void Start()
    {

    }

    void Update()
    {
        CheckCollision();

        CheckInput();

        if (isRunning)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
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
            isRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        // 레이캐스트 렌더링
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
