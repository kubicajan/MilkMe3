using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float speed = 20f;
    public Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            Debug.Log("hit enemy");
            enemy.TakeDamage(10);
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        enabled = false;
        Destroy(gameObject);
    }
}
