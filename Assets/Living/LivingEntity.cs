using System.Collections;
using UnityEngine;

namespace Living
{
	public abstract class LivingEntity : MonoBehaviour
	{
		[SerializeField] public LayerMask hostileLayers;
		[SerializeField] public LayerMask groundLayers;
		protected Rigidbody2D RigidBody { get; private set; }
		protected BoxCollider2D BoxCollider { get; private set; }
		public Coroutine movementCoroutine;

		public ParticleSystem deathParticleEffect;
		private bool Immobilized = false;
		protected bool dead = false;

		private bool immuneToKnockBackX = false;
		private int currentHealth;
		private int maximumHealth;

		//todo: this should be done universally. A unit should have a list of things that it is immune/damagable by
		//todo: it should check before triggering any damage
		private int CurrentHealth
		{
			get => currentHealth;
			set
			{
				currentHealth = value;
				Debug.Log($"{gameObject.name} has {currentHealth} health remaining");
				if (currentHealth <= 0 && !dead)
				{
					dead = true;
					Die();
				}
			}
		}

		public virtual void Die()
		{
			StartCoroutine(DieCoroutine());
		}

		protected IEnumerator DieCoroutine(float secondsToDeath = 0)
		{
			Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
			yield return new WaitForSeconds(secondsToDeath);
			gameObject.SetActive(false);
		}

		protected void Init(int _health, Rigidbody2D _rigidBody2D, BoxCollider2D _boxCollider)
		{
			maximumHealth = _health;
			currentHealth = maximumHealth;
			BoxCollider = _boxCollider;
			RigidBody = _rigidBody2D;
		}

		public void Immobilize(bool immobilize)
		{
			Immobilized = immobilize;
		}

		protected bool IsImmobilized()
		{
			return Immobilized;
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
}