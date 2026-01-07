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
			const float SPEED = 15f;
			const float SPEED_VARIANCE = 1.5f;
			const float UPWARD_FORCE = 5f;
			const float UPWARD_VARIANCE = 1f;
			float finalSpeed = SPEED + Random.Range(-SPEED_VARIANCE, SPEED_VARIANCE);
			float finalUpward = UPWARD_FORCE + Random.Range(-UPWARD_VARIANCE, UPWARD_VARIANCE);

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
			Instantiate(hitParticle, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}