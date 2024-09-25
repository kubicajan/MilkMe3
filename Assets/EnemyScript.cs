using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health = 100;

    public Rigidbody2D rigidBody2D;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{health} health remaining");
    }

    public void GetKnockbacked(Vector3 perpetratorPosition)
    {
        Vector2 direction = (transform.position - perpetratorPosition).normalized;
        Debug.Log(direction);
        Vector2 force = direction * 2;
        force.y = 10;
        rigidBody2D.AddForce(force, ForceMode2D.Impulse); //if you don't want to take into consideration enemy's mass then use ForceMode.VelocityChange
    }
}
