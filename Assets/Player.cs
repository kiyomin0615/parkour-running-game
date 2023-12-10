﻿using System;
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
    [SerializeField] LayerMask layerMask;

    [Header("Collision Info")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;

    bool playerUnlocked;
    bool isGrounded;
    bool canDoubleJump;
    bool wallDeteced;
    bool isSliding;

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

        CheckSlideTimer();

        checkSlideCooldownTimer();

        if (playerUnlocked && !wallDeteced)
        {
            PlayerMove();
        }
    }



    private void PlayerMove()
    {
        if (isSliding)
        {
            myRigidbody.velocity = new Vector2(slideSpeed, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
    }

    private void AnimatorController()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);
        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetFloat("xVelocity", myRigidbody.velocity.x);
        animator.SetFloat("yVelocity", myRigidbody.velocity.y);
    }

    private void CheckCollision()
    {
        // 레이캐스트
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, layerMask);

        // 박스캐스트
        wallDeteced = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, layerMask);
    }

    private void checkSlideCooldownTimer()
    {
        slideCooldownTimer -= Time.deltaTime;
    }

    private void CheckSlideTimer()
    {
        sliderTimer -= Time.deltaTime;

        if (sliderTimer < 0)
        {
            isSliding = false;
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

    private void OnDrawGizmos()
    {
        // 레이캐스트 렌더링
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        // 박스캐스트 렌더링
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
