using System.Collections;
using UnityEngine;

public class EnemyScript : LivingEntity
{
    public Transform groundCheck;
    public LayerMask groundLayers;

    private void Start()
    {
        Init(_health: 500,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }

    public void LiftMeUp(int liftByThisMuch)
    {
        StartCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform));
    }

    public void ThrowDown(int liftByThisMuch)
    {
        StartCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform));
    }

    public void AttackMoveMe(int moveBy, float directionToMove)
    {
        StartCoroutine(MoveAttack(transform.position.x, moveBy, directionToMove));
    }


    private IEnumerator MoveAttack(float positionBeforeMoveX, int moveBy, float directionToMove)
    {
        if (directionToMove > 0)
        {
            while (positionBeforeMoveX + moveBy > transform.position.x)
            {
                RigidBody.velocity = new Vector2(15, 0);
                yield return null;
            }
        }
        else
        {
            while (positionBeforeMoveX - moveBy < transform.position.x)
            {
                RigidBody.velocity = new Vector2(-15, 0);
                yield return null;
            }
        }

        RigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
    }

    public void StompMeDown(int stompSpeed)
    {
        StartCoroutine(StompDown(stompSpeed));
    }

    private IEnumerator StompDown(int stompSpeed)
    {

        while (!IsGrounded())
        {
            RigidBody.velocity = new Vector2(0, stompSpeed);
            Debug.Log("still stomping");
            yield return null;
        }
        RigidBody.velocity = Vector2.zero;
    }

    protected bool IsGrounded()
    {
        return Utility.IsGroundedOnLayers(groundCheck.position, groundLayers);
    }
}
