using System;
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

    [Header("Slide Info")]
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    [SerializeField] float sliderTimer;
    [SerializeField] float slideCooldownTime;
    [SerializeField] float slideCooldownTimer;

    [Header("Raycast Info")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] float ceilingCheckDistance;
    [SerializeField] LayerMask layerMask;

    [Header("Collision Info")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;

    [Header("Ledge Info")]
    [SerializeField] Vector2 offset1;
    [SerializeField] Vector2 offset2;
    Vector2 climbStartPosition;
    Vector2 climbEndPosition;

    bool playerUnlocked;
    bool isGrounded;
    bool canDoubleJump;
    bool wallDeteced;
    bool isSliding;
    bool ceilingDetected;
    bool canGrabLedge = true;
    bool isClimbing;

    [HideInInspector] public bool ledgeDetected;

    void Start()
    {
        // Rigidbody2D 컴포넌트 참조
        myRigidbody = GetComponent<Rigidbody2D>();
        // Animator 컴포넌트 참조
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        AnimatorController();

        CheckCollision();

        CheckInput();

        CheckSlide();

        CheckLedge();

        checkSlideCooldownTimer();

        if (playerUnlocked)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        if (wallDeteced)
        {
            return;
        }

        if (isSliding)
        {
            myRigidbody.velocity = new Vector2(slideSpeed, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
    }

    private void CheckLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbStartPosition = ledgePosition + offset1;
            climbEndPosition = ledgePosition + offset2;

            isClimbing = true;
        }

        if (isClimbing)
        {
            transform.position = climbStartPosition;
        }
    }

    // Animation Clip에서 이벤트 핸들러로 등록
    private void ClimbOver()
    {
        isClimbing = false;
        transform.position = climbEndPosition;
        Invoke("ResetGrabState", 0.5f);

    }

    private void ResetGrabState()
    {
        canGrabLedge = true;
    }

    private void CheckSlide()
    {
        sliderTimer -= Time.deltaTime;

        if (sliderTimer < 0 && !ceilingDetected)
        {
            isSliding = false;
        }
    }

    private void checkSlideCooldownTimer()
    {
        slideCooldownTimer -= Time.deltaTime;
    }

    private void Slide()
    {
        if (myRigidbody.velocity.x > 0 && slideCooldownTimer <= 0)
        {
            isSliding = true;
            sliderTimer = slideTime;
            slideCooldownTimer = slideCooldownTime;
        }
    }

    private void Jump()
    {
        if (isSliding)
        {
            return;
        }

        if (isGrounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, singleJumpForce);
        }
        else if (canDoubleJump)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, doubleJumpForce);
            canDoubleJump = false;
        }
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide();
        }
    }

    private void AnimatorController()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);
        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetFloat("xVelocity", myRigidbody.velocity.x);
        animator.SetFloat("yVelocity", myRigidbody.velocity.y);
    }

    private void CheckCollision()
    {
        // 레이캐스트
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, layerMask);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, layerMask);

        // 박스캐스트
        wallDeteced = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, layerMask);
    }

    private void OnDrawGizmos()
    {
        // 레이캐스트 렌더링
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));

        // 박스캐스트 렌더링
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
