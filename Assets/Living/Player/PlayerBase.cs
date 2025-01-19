using UnityEngine;

public class PlayerBase : LivingEntity
{
    public Transform attackPoint;
    public Transform groundCheck;
    public LayerMask groundLayers;
    public LayerMask buildingLayers;
    public LayerMask enemyLayers;
    public LayerMask npcLayers;

    void Awake()
    {
       var gg = GetComponent<Rigidbody2D>();
        Init(_health: 100,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }

    public Rigidbody2D GetRigidBody()
    {
        return RigidBody;
    }
}