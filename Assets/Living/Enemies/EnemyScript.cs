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
        StartCoroutine(Common.WarriorMoveAttack(this.transform.position.x, moveBy, directionToMove, transform, RigidBody));

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
