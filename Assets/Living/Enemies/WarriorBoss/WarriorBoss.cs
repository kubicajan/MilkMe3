using System.Collections;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBoss : EnemyScript
	{
		[SerializeField] private Transform attackPoint;
		[SerializeField] public GameObject heavyRangeAttack;
		private const float MELEE_ATTACK_RANGE = 3f;

		private void Awake()
		{
			movementSpeed = 9f;
			gameObject.tag = GameTag.Npc;
		}

		public void HeavyAttack()
		{
			MeleeAttack();
			Instantiate(heavyRangeAttack, transform.position, Quaternion.identity);
		}

		public void DoubleAttack()
		{
			MeleeAttack();
		}

		private void MeleeAttack()
		{
			const float KNOCKBACK = 2;
			DealDamageTo(DetectHostilesInRange(MELEE_ATTACK_RANGE), KNOCKBACK);
		}

		public override void Die()
		{
			GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Death);
			StartCoroutine(DieCoroutine(5));
		}


		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Annoyed);
			gameObject.tag = GameTag.Boss;
		}

		public bool CanAttack()
		{
			return Vector2.Distance(playerLocation.position, attackPoint.position) <= MELEE_ATTACK_RANGE;
		}

		public string SelectAttack()
		{
			if (GetRandomOneOrTwo() != 1)
			{
				return WarriorBossTrigger.HeavyAttack;
			}
			else
			{
				return WarriorBossTrigger.DoubleAttack;
			}
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

		private Collider2D[] DetectHostilesInRange(float range)
		{
			return Utility.DetectByLayers(attackPoint.position, range, hostileLayers);
		}
	}
}