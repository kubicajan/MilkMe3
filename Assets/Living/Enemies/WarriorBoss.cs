using UnityEngine;

namespace Living.Enemies
{
	public class WarriorBoss : EnemyScript
	{
		[SerializeField] private Transform attackPoint;
		public LayerMask enemyLayers;

		private const float MEELE_ATTACK_RANGE = 3f;

		private void Awake()
		{
			speed = 9f;
		}


		public void MeeleAttack()
		{
			const float KNOCKBACK = 2;
			DealDamageTo(DetectEnemiesInRange(MEELE_ATTACK_RANGE), KNOCKBACK);
		}

		public bool ShouldAttack()
		{
			return Vector2.Distance(playerLocation.position, attackPoint.position) < 5f;
		}

		private void DealDamageTo(Collider2D[] detectedEnemies, float knockBack)
		{
			foreach (Collider2D enemy in detectedEnemies)
			{
				if (enemy.TryGetComponent<LivingEntity>(out var enemyScript))
				{
					enemyScript.TakeDamage(10);
					enemyScript.GetKnockedBack(this.transform.position, knockBack);
					Debug.Log("hit something");
				}
			}
		}

		private Collider2D[] DetectEnemiesInRange(float range)
		{
			return Utility.DetectByLayers(attackPoint.position, range, enemyLayers);
		}
	}
}