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
        EnemyScript enemyScript = collision.GetComponent<EnemyScript>();
        if (enemyScript != null)
        {
            Debug.Log("hit enemy");
            enemyScript.GetKnockedBack(this.transform.position);
            enemyScript.TakeDamage(10);
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        enabled = false;
        Destroy(gameObject);
    }
}
