using UnityEngine;

public class EnemyScript : LivingEntity
{
    private void Start()
    {
        Init(_health: 100,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }
}
