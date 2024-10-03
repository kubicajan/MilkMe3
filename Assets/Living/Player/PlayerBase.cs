using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerBase : LivingEntity
{
    public Transform attackPoint;
    public LineRenderer laser;
    public GameObject bulletPrefab;
    public Transform groundCheck;
    public LayerMask groundLayers;
    public LayerMask buildingLayers;
    public LayerMask enemyLayers;

    void Start()
    {
        Init(_health: 100,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }

    public Rigidbody2D GetRigidBody()
    {
        return RigidBody;
    }
}