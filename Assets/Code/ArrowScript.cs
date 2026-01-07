using Helpers.CommonEnums;
using UnityEngine;

namespace Code
{
	public class ArrowScript : MonoBehaviour
	{
		[SerializeField] private ParticleSystem hitParticle;
		[SerializeField] private Rigidbody2D rigidBody;

		public static void InitializeAutoAttack(ArrowScript prefab, Vector3 position, Quaternion rotation,
			float direction)
		{
			float speed = 15f;
			float speedVariance = 1.5f;
			float upwardForce = 5f;
			float upwardVariance = 1f;
			float finalSpeed = speed + Random.Range(-speedVariance, speedVariance);
			float finalUpward = upwardForce + Random.Range(-upwardVariance, upwardVariance);

			ArrowScript arrow = Instantiate(prefab, position, rotation);
			arrow.rigidBody.linearVelocity = (arrow.transform.right * (finalSpeed * direction)) +
			                                 (arrow.transform.up * finalUpward);
		}

		public static void InitializeDrop(ArrowScript prefab, Vector3 position, Quaternion rotation)
		{
			ArrowScript arrow = Instantiate(prefab, position, rotation);
			arrow.rigidBody.gravityScale = 5f;
		}

		private void FixedUpdate()
		{
			if (rigidBody.linearVelocity.sqrMagnitude > 0.01f)
			{
				float angle = Mathf.Atan2(rigidBody.linearVelocity.y, rigidBody.linearVelocity.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			// EnemyScript enemyScript = collision.GetComponent<EnemyScript>();
			// if (enemyScript != null)
			// {
			// 	Debug.Log("hit enemy");
			// 	// enemyScript.GetKnockedBack(this.transform.position, 2);
			// 	enemyScript.TakeDamage(10);
			// 	Destroy(gameObject);
			// }

			if (collision.gameObject.CompareTag(GameTag.PlayerProjectile))
			{
				return;
			}
			Instantiate(hitParticle, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}