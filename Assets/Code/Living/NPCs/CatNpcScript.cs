using UnityEngine;

namespace Living.NPCs
{
	public class CatNpcScript : NpcScript
	{
		[SerializeField] private Rigidbody2D rb;
		private float moveSpeed = 2f;
		private float changeDirectionTime = 2f;

		private float timer;
		private int direction;

		private void Update()
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				ChooseDirection();
			}
		}

		private void FixedUpdate()
		{
			rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
		}

		private void ChooseDirection()
		{
			direction = Random.Range(0, 2) == 0 ? -1 : 1;
			timer = changeDirectionTime;
			FlipSprite();
		}

		private void FlipSprite()
		{
			transform.localScale = new Vector3(direction, 1, 1);
		}
	}
}