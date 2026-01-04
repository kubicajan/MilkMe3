using System.Collections;
using UnityEngine;

namespace Living
{
	public abstract class LivingEntity : MonoBehaviour
	{
		[SerializeField] public LayerMask hostileLayers;
		[SerializeField] public LayerMask groundLayers;
		[SerializeField] public LayerMask itemLayers;
		protected Rigidbody2D RigidBody { get; private set; }
		protected BoxCollider2D BoxCollider { get; private set; }
		public Coroutine movementCoroutine;

		public ParticleSystem deathParticleEffect;
		private bool isImmobilized = false;
		private bool isDead = false;
		public bool isAttacking = false;

		private bool immuneToKnockBackX = false;
		private int currentHealth;
		private int maximumHealth;

		private int currentShieldHealth = 0;
		private ParticleSystem currentShieldParticleEffect;
		private CircleCollider2D currentShieldCollider;

		//todo: this should be done universally. A unit should have a list of things that it is immune/damagable by
		//todo: it should check before triggering any damage
		private int CurrentHealth
		{
			get => currentHealth;
			set
			{
				currentHealth = value;
				Debug.Log($"{gameObject.name} has {currentHealth} health remaining");
				if (currentHealth <= 0 && !isDead)
				{
					isDead = true;
					Die();
				}
			}
		}

		private int CurrentShieldHealth
		{
			get => currentShieldHealth;
			set
			{
				currentShieldHealth = value;
				Debug.Log($"{gameObject.name} has {currentShieldHealth} shield remaining");
				if (currentShieldHealth <= 0 && !isDead)
				{
					currentShieldParticleEffect.Stop();
					currentShieldCollider.enabled = false;
					Debug.Log($"Shield broke!");
				}
			}
		}

		public void ActivateShield(int shieldHp, ParticleSystem shieldParticleEffect, CircleCollider2D shieldCollider)
		{
			currentShieldHealth += shieldHp;
			currentShieldParticleEffect = shieldParticleEffect;
			currentShieldCollider = shieldCollider;

			currentShieldParticleEffect.Play();
			currentShieldCollider.enabled = true;
		}

		protected virtual void Die()
		{
			StartCoroutine(DieCoroutine());
		}

		private IEnumerator DieCoroutine(float secondsToDeath = 0)
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
			isImmobilized = immobilize;
		}

		protected bool IsImmobilized()
		{
			return isImmobilized;
		}

		public void TakeDamage(int damage)
		{
			if (currentShieldHealth > 0)
			{
				CurrentShieldHealth -= damage;
			}
			else
			{
				CurrentHealth -= damage;
			}
		}

		public int GetCurrentHealth()
		{
			return CurrentHealth;
		}

		public void Heal(int heal)
		{
			CurrentHealth += heal;
		}

		private Coroutine coroutine;

		//todo: tyhle dve metody jsou uplne stejne, klidne by stacilo pouzit jednu
		public void GetKnockedBack(Vector2 perpetratorPosition, float knockbackDistance)
		{
			if (coroutine == null)
			{
				coroutine = StartCoroutine(MagicPushMeCoroutine(perpetratorPosition, knockbackDistance));
			}
		}

		public void MagicPushMe(Vector2 perpetratorPosition, float knockbackDistance)
		{
			if (coroutine == null)
			{
				StartCoroutine(MagicPushMeCoroutine(perpetratorPosition, knockbackDistance));
			}
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

			Vector2 direction = (transform.position - (Vector3)perpetratorPosition).normalized;
			Vector2 targetPosition = (Vector2)transform.position + direction * knockbackDistance;
			targetPosition.y = originalY;

			while (timer > 0)
			{
				transform.position = Vector2.MoveTowards(transform.position, targetPosition,
					knockbackDistance / 0.3f * Time.deltaTime);
				timer -= Time.deltaTime;
				yield return null;
			}

			//outdated, kept just in case:

			// while (timer > 0)
			// {
			// 	Vector2 objectPosition = transform.position;
			// 	Vector2 distanceToEnemy = objectPosition - perpetratorPosition;
			//
			// 	if (Vector2.Distance(transform.position,
			// 		    perpetratorPosition + distanceToEnemy.normalized * knockbackDistance) < 0.2f)
			// 	{
			// 		immuneToKnockBackX = false;
			// 		yield break;
			// 	}
			//
			// 	Vector2 targetPosition = perpetratorPosition + distanceToEnemy.normalized * knockbackDistance;
			// 	targetPosition.y = originalY;
			// 	float speed = (Mathf.Abs(knockbackDistance) - Mathf.Abs(distanceToEnemy.x)) * 10;
			// 	Debug.Log(targetPosition.x);
			// 	transform.position = Vector3.MoveTowards(objectPosition, targetPosition, speed * Time.deltaTime);
			// 	timer -= Time.deltaTime;
			// 	yield return null;
			// }

			immuneToKnockBackX = false;
			coroutine = null;
		}
	}
}