using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] Player player;
    [SerializeField] LayerMask layerMask;

    bool canDetect;

    private void Update()
    {
        if (canDetect)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, layerMask);
        }
    }

    // 벽이 너무 높은 경우
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
