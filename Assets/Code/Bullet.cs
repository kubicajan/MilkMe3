using Helpers.CommonEnums;
using Living;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public ParticleSystem explosionParticles;
	private float speed = 30;
	private float direction = 1f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;

		if (CompareTag(GameTag.Enemy) || CompareTag(GameTag.Boss))
		{
			if (other.CompareTag(GameTag.Player))
			{
				LivingEntity entity = other.GetComponent<LivingEntity>();
				Instantiate(explosionParticles, transform.position, Quaternion.identity);
				Destroy(gameObject);
				entity.TakeDamage(10);
			}
		}
		else if (CompareTag(GameTag.PlayerProjectile))
		{
			if (other.CompareTag(GameTag.Boss) || other.CompareTag(GameTag.Enemy))
			{
				LivingEntity entity = other.GetComponent<LivingEntity>();
				Instantiate(explosionParticles, transform.position, Quaternion.identity);
				Destroy(gameObject);
				entity.TakeDamage(10);
			}
		}
	}

	void Update()
	{
		Vector3 move = new Vector3(direction, 0, 0f);
		transform.Translate(move * speed * Time.deltaTime);
	}
}