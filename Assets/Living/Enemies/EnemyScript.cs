using UnityEngine;

public class EnemyScript : LivingEntity
{
    private void Start()
    {
        Init(_health: 500,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }
}
