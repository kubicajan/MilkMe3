using System.Collections;
using UnityEngine;

public class EnemyScript : LivingEntity
{
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
}
