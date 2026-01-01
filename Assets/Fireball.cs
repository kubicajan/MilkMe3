using UnityEngine;

public class Fireball : MonoBehaviour
{
	[SerializeField] private ParticleSystem explosion;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}