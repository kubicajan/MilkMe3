using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public partial class PlayerController
{
    private float moveSpeed = 10f;
    private float jumpForce = 20f;
    private float dashForce = 100f;
    private Vector2 movement;
    private BoxCollider2D boxCollider;
    bool dashing = false;
    private float lastDirection = 1;
    private int consecutiveJumps = 1;

    private void Move()
    {
        if (lastDirection != movement.x)
        {
            lastDirection = movement.x;
            transform.Rotate(0f, 180f, 0f);
        }

        Vector3 m_Velocity = Vector3.zero;
        Vector3 targetVelocity = new Vector2(movement.x * moveSpeed, RigidBody.velocity.y);
        RigidBody.velocity = Vector3.SmoothDamp(RigidBody.velocity, targetVelocity, ref m_Velocity, .05f);

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
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);
    }

    private void DisableEnemyCollision(bool disable)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, Mathf.RoundToInt(Mathf.Log(enemyLayers.value, 2)), disable);
    }

    private IEnumerator Dash()
    {
        ResetJumps();
        DisableEnemyCollision(true);
        float originalGravity = RigidBody.gravityScale;
        dashing = true;
        RigidBody.gravityScale = 0;
        RigidBody.velocity = new Vector2(lastDirection * dashForce, 0);
        yield return new WaitForSeconds(0.1f);
        RigidBody.velocity = new Vector2();
        yield return new WaitForSeconds(0.1f);
        dashing = false;
        RigidBody.gravityScale = originalGravity;
        DisableEnemyCollision(false);
    }
}
