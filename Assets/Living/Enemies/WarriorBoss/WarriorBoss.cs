using Unity.VisualScripting;
using UnityEngine;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBoss : EnemyScript
	{
		[SerializeField] private Transform attackPoint;
		public LayerMask enemyLayers;
		private const float MEELE_ATTACK_RANGE = 3f;

		private void Awake()
		{
			movementSpeed = 9f;
		}

		public void MeeleAttack()
		{
			const float KNOCKBACK = 2;
			DealDamageTo(DetectEnemiesInRange(MEELE_ATTACK_RANGE), KNOCKBACK);
		}

		public string SelectAttack()
		{
			if (Vector2.Distance(playerLocation.position, attackPoint.position) <= MEELE_ATTACK_RANGE)
			{
				if (GetRandomOneOrTwo() != 1)
				{
					return nameof(WarriorBossTriggersEnum.HEAVY_ATTACK);
				}
				else
				{
					return nameof(WarriorBossTriggersEnum.DOUBLE_ATTACK);
				}
			}
			return null;
		}

		private int GetRandomOneOrTwo()
		{
			return Random.Range(1, 3); // Random.Range with (1,3) returns either 1 or 2
		}

		private void DealDamageTo(Collider2D[] detectedEnemies, float knockBack)
		{
			foreach (Collider2D enemy in detectedEnemies)
			{
				if (enemy.TryGetComponent<LivingEntity>(out var enemyScript))
				{
					enemyScript.TakeDamage(10);
					enemyScript.GetKnockedBack(this.transform.position, knockBack);
				}
			}
		}

		private Collider2D[] DetectEnemiesInRange(float range)
		{
			return Utility.DetectByLayers(attackPoint.position, range, enemyLayers);
		}
	}
}