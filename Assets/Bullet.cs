using System.Collections;
using System.Collections.Generic;
using Helpers.CommonEnums;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public ParticleSystem explosionParticles;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (CompareTag(GameTag.Enemy) || CompareTag(GameTag.Boss))
		{
			if (collision.gameObject.CompareTag(GameTag.Player))
			{
				Instantiate(explosionParticles, transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
		}
	}

	public float speed = 5000f;
	private float direction = 10f;
	private float timer = 0f;
	public float switchTime = 1f;

	void Update()
	{
		// Move the object
		Vector3 move = new Vector3(direction, 0, 0f);
		transform.Translate(move * speed * Time.deltaTime);

		// Update timer
		timer += Time.deltaTime;

		// Switch direction every `switchTime` seconds
		if (timer >= switchTime)
		{
			direction *= -1f; // flip direction
			timer = 0f; // reset timer
		}
	}
}