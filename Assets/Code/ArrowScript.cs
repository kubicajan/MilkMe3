using UnityEngine;

namespace Code
{
	public class ArrowScript : MonoBehaviour
	{
		private float speed = 12f;
		private float speedVariance = 1.5f;

		private float upwardForce = 5f;
		private float upwardVariance = 1f;

		private float direction = 1f;
		[SerializeField] private ParticleSystem hitParticle;

		public Rigidbody2D rigidBody;

		private void Start()
		{
			float finalSpeed = speed + Random.Range(-speedVariance, speedVariance);
			float finalUpward = upwardForce + Random.Range(-upwardVariance, upwardVariance);

			rigidBody.linearVelocity = (transform.right * finalSpeed * direction) + (transform.up * finalUpward);
		}

		public void Instantiate(float _direction)
		{
			direction = _direction;
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