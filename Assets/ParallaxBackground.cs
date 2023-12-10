using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallax;

    GameObject mainCamera;

    float length;
    float xPos;

    void Start()
    {
        // GameObject.Find 메소드를 절대 Update 안에서 호출하지 마라
        mainCamera = GameObject.Find("Main Camera");

        // 백그라운드 너비
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPos = transform.position.x;
    }

    void Update()
    {
        float distanceToMove = mainCamera.transform.position.x * parallax;
        float distanceToCamera = mainCamera.transform.position.x - distanceToMove;

        // 백그라운드가 카메라를 따라간다
        transform.position = new Vector2(xPos + distanceToMove, transform.position.y);

        // 백그라운드와 카메라 사이의 거리가 멀어져서 카메라 화면 바깥으로 나가면,
        // 다시 백그라운드를 카메라 화면 안으로 넣는다
        if (distanceToCamera > xPos + length)
        {
            xPos += length;
        }
    }
}
