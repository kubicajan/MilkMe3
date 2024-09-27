
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    private int health;
    protected Rigidbody2D rigidBody;

    public void Initialize(int _health, Rigidbody2D _rigidBody2D)
    {
        health = _health;
        rigidBody = _rigidBody2D;
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
        rigidBody.AddForce(force, ForceMode2D.Impulse); //if you don't want to take into consideration enemy's mass then use ForceMode.VelocityChange
    }
}