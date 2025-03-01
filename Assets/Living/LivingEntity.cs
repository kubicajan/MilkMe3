
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    protected Rigidbody2D RigidBody { get; private set; }
    protected BoxCollider2D BoxCollider { get; private set; }
    public Coroutine movementCoroutine;

    public ParticleSystem deathParticleEffect;
    private bool Immobilized = false;

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

    public void Immobilize(bool immobilize)
    {
        Immobilized = immobilize;
    }

    public bool IsImmobilized()
    {
        return Immobilized;
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

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void Heal(int heal)
    {
        CurrentHealth += heal;
    }

    //TODO:
    //ten knockback to picuje s tema projektilama, mozna to udelat jako magicPush, ten funguje
    public void GetKnockedBack(Vector2 perpetratorPosition, float knockbackDistance)
    {
        Immobilize(true);
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

    public void MagicPushMe(Vector2 perpetratorPosition, float knockbackDistance)
    {
        StartCoroutine(MagicPushMeCoroutine(perpetratorPosition, knockbackDistance));
    }

    private IEnumerator MagicPushMeCoroutine(Vector2 perpetratorPosition, float knockbackDistance)
    {
        if (immuneToKnockBackX)
        {
            yield break;
        }
        float originalY = gameObject.transform.position.y;
        float timer = 0.3f;
        immuneToKnockBackX = true;

        while (timer > 0)
        {
            Vector2 objectPosition = transform.position;
            Vector2 distanceToEnemy = objectPosition - perpetratorPosition;

            if (Mathf.Abs(distanceToEnemy.x - knockbackDistance) < 0.2f)
            {
                immuneToKnockBackX = false;
                yield break;
            }
            Vector2 targetPosition = perpetratorPosition + distanceToEnemy.normalized * knockbackDistance;
            targetPosition.y = originalY;
            float speed = (Mathf.Abs(knockbackDistance) - Mathf.Abs(distanceToEnemy.x)) * 10;
            transform.position = Vector3.MoveTowards(objectPosition, targetPosition, speed * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        immuneToKnockBackX = false;
    }
}