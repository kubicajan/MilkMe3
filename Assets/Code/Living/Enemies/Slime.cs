using UnityEngine;

namespace Code.Living.Enemies
{
	public class Slime : EnemyScript
	{
		public void Jump()
		{
			TurnTowardsTarget();
			Vector2 direction = (playerLocation.position - transform.position).normalized;
			GetRigidBody().linearVelocity = new Vector2(direction.x * 5, 15);
		}
	}
}