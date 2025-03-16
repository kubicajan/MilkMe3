using System;
using UnityEngine;

namespace Living.Enemies
{
	public class Slime : EnemyScript
	{
		void Jump()
		{
			if (RigidBody.velocity.y == 0) // Ensures jumping only when grounded
			{
				Vector2 direction = (playerLocation.position - transform.position).normalized;
				RigidBody.velocity = new Vector2(direction.x * 5, 15);
			}
		}
	}
}