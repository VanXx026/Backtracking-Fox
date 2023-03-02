using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask ground;
    public float moveSpeed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //FIXME:有问题，暂时先不写
        // GroundMovement();
    }

    private void GroundMovement()
    {
        RaycastHit2D rh = Physics2D.Raycast(transform.position, -Vector2.up, 1f, ground);
        // Physics2D.queriesStartInColliders = false;
        // rb.velocity = new Vector2(moveSpeed * transform.localScale.x, rb.velocity.y);
        // Debug.Log(transform.localScale.x);
        if (rh.collider == null)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * transform.localScale.x, rb.velocity.y);
        }
    }
}
