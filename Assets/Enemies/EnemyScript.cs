using UnityEngine;

public class EnemyScript : LivingEntity
{
    private void Start()
    {
        Initialize(100, GetComponent<Rigidbody2D>());
    }
}
