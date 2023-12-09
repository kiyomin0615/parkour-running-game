using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform[] levelParts;
    [SerializeField] Transform respawnPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Transform randomPart = levelParts[Random.Range(0, levelParts.Length)];
            Transform newPart = Instantiate(randomPart, respawnPosition.position, respawnPosition.rotation, transform).transform;
        }
    }
}
