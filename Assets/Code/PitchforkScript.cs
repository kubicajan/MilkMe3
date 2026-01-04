using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchforkScript : MonoBehaviour
{
    private float speed = 25f;
    public Rigidbody2D rigidBody;
    public AnimationCurve curve;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // rigidBody.velocity = new Vector2((transform.right * speed).x, 15);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // EnemyScript enemyScript = collision.GetComponent<EnemyScript>();
        // if (enemyScript != null)
        // {
        //     Debug.Log("hit enemy");
        //     enemyScript.GetKnockedBack(this.transform.position, 2);
        //     enemyScript.TakeDamage(10);
        //     Destroy(gameObject);
        // }
    }
    // void OnBecameInvisible()
    // {
    //     enabled = false;
    //     Destroy(gameObject);
    // }
}
