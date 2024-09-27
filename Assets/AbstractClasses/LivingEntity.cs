
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    protected Rigidbody2D RigidBody { get; private set; }
    protected BoxCollider2D BoxCollider { get; private set; }

    public ParticleSystem deathParticleEffect;

    private bool immuneToKnockBackX = false;
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

    public void GetKnockedBack(Vector2 perpetratorPosition, float knockbackDistance)
    {
        Vector2 direction = ((Vector2)transform.position - perpetratorPosition).normalized;
        Vector2 force = new Vector2();

        if (!immuneToKnockBackX)
        {
            force.x = direction.x * knockbackDistance;
            Debug.Log(knockbackDistance);
        }
        force.y = knockbackDistance * RigidBody.gravityScale;
        RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public IEnumerator MoveEnemyCoroutine(Vector2 perpetratorPosition, float knockbackDistance)
    {
        if (immuneToKnockBackX)
        {
            yield break;
        }

        float targetDistance = knockbackDistance;
        float timer = 0.3f;
        immuneToKnockBackX = true;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Vector2 directionToEnemy = (Vector2)transform.position - perpetratorPosition;
            float currentDistance = directionToEnemy.x;

            if (Mathf.Abs(currentDistance - targetDistance) < 0.2f)
            {
                immuneToKnockBackX = false;
                yield break;
            }
            Vector2 directionNormalized = directionToEnemy.normalized;
            Vector2 targetPosition = perpetratorPosition + directionNormalized * targetDistance;
            targetPosition.y = transform.position.y;
            float speed = (Mathf.Abs(targetDistance) - Mathf.Abs(currentDistance)) * 10;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        immuneToKnockBackX = false;
    }
}