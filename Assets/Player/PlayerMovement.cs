using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public partial class PlayerScript : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float jumpForce = 20f;
    private float dashForce = 100f;
    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    bool dashing = false;
    private float lastDirection = 1;
    private int consecutiveJumps = 1;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Move()
    {
        if (lastDirection != movement.x)
        {
            lastDirection = movement.x;
            transform.Rotate(0f, 180f, 0f);
        }

        Vector3 m_Velocity = Vector3.zero;
        Vector3 targetVelocity = new Vector2(movement.x * moveSpeed, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref m_Velocity, .05f);

    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayers);
        bool isGrounded = colliders.Length > 0;
        if (isGrounded)
        {
            ResetJumps();
        }
        return isGrounded;
    }

    private void ResetJumps()
    {
        consecutiveJumps = 0;
    }

    private void Jump()
    {
        const int MAX_JUMPS = 5;
        if (!IsGrounded() && MAX_JUMPS <= consecutiveJumps)
        {
            return;
        }
        consecutiveJumps++;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    private void DisableEnemyCollision(bool disable)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, Mathf.RoundToInt(Mathf.Log(enemyLayers.value, 2)), disable);
    }

    private IEnumerator Dash()
    {
        ResetJumps();
        DisableEnemyCollision(true);
        float originalGravity = rigidBody.gravityScale;
        dashing = true;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(lastDirection * dashForce, 0);
        yield return new WaitForSeconds(0.1f);
        rigidBody.velocity = new Vector2();
        yield return new WaitForSeconds(0.1f);
        dashing = false;
        rigidBody.gravityScale = originalGravity;
        DisableEnemyCollision(false);
    }
}
