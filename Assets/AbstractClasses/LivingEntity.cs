
using Unity.VisualScripting;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    protected Rigidbody2D RigidBody { get; private set; }
    protected BoxCollider2D BoxCollider { get; private set; }

    public ParticleSystem deathParticleEffect;

    private int currentHealth;
    private int maximumHealth;
    private int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            Debug.Log($"{gameObject.name} has {currentHealth} health remaining");
            if (currentHealth <= 0)
            {
                Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }

    public void Init(int _health, Rigidbody2D _rigidBody2D, BoxCollider2D _boxCollider)
    {
        maximumHealth = _health;
        currentHealth = maximumHealth;
        BoxCollider = _boxCollider;
        RigidBody = _rigidBody2D;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    protected int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    private void Heal(int heal)
    {
        CurrentHealth += heal;
    }

    public void GetKnockedBack(Vector3 perpetratorPosition)
    {
        Vector2 direction = (transform.position - perpetratorPosition).normalized;
        Vector2 force = direction * 2;
        force.y = 10;
        RigidBody.AddForce(force, ForceMode2D.Impulse); //if you don't want to take into consideration enemy's mass then use ForceMode.VelocityChange
    }
}