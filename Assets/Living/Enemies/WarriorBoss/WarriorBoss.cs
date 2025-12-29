using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBoss : EnemyScript
	{
		[SerializeField] private Transform attackPoint;
		[SerializeField] private ParticleSystem godRays;
		[SerializeField] public GameObject heavyRangeAttack;
		[SerializeField] private Animator animator;
		private const float MELEE_ATTACK_RANGE = 3f;

		private void Awake()
		{
			movementSpeed = 9f;
			gameObject.tag = GameTag.Npc;
		}

		public void FixedUpdate()
		{
			if (!isAttacking && CanAttack())
			{
				animator.SetTrigger(SelectAttack());
			}
		}

		public void HeavyAttack()
		{
			const float KNOCKBACK = 20;
			MeleeAttack(KNOCKBACK);
			Instantiate(heavyRangeAttack, transform.position, Quaternion.identity);
		}

		public void LightAttack()
		{
			MeleeAttack(0);
		}

		public void MediumAttack()
		{
			MeleeAttack(10);
		}

		private void MeleeAttack(float knockback = 0)
		{
			DealDamageTo(DetectHostilesInRange(MELEE_ATTACK_RANGE), knockback);
		}

		public override void Die()
		{
			GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Death);
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
			StartCoroutine(DieCoroutine());
		}

		public IEnumerator DieCoroutine()
		{
			yield return new WaitForSeconds(1f);
			godRays.Play();
			gameObject.layer = LayerMask.NameToLayer(GameLayer.Prop);
			gameObject.tag = GameTag.Prop;
			//TODO:THIS WHOLE THING COULD BE REPLACED WITH AN IMAGE
			GetComponent<Rigidbody2D>().simulated = false;
			StartCoroutine(ItemManager.Instance.SpawnItems(7, transform.position));
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Annoyed);
			// GetComponent<Animator>().SetTrigger(WarriorBossTrigger.SecondStage);

			gameObject.tag = GameTag.Boss;
		}

		private bool CanAttack()
		{
			//check if angry & close enough to attack
			return gameObject.CompareTag(GameTag.Boss)
				? Vector2.Distance(playerLocation.position, attackPoint.position) <= MELEE_ATTACK_RANGE
				: false;
		}

		private string SelectAttack()
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

		public void Teleport(Vector2 position)
		{
			transform.position = position;
		}

		public void Fly(int direction, float speed)
		{
			Vector3 targetPosition =
				new Vector3(transform.position.x, transform.position.y + (10 * direction), transform.position.z);
			transform.position =
				Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		}

		private int GetRandomOneOrTwo()
		{
			return Random.Range(1, 3); // Random.Range with (1,3) returns either 1 or 2
		}

		private void DealDamageTo(Collider2D[] detectedTargets, float knockBack)
		{
			foreach (Collider2D target in detectedTargets)
			{
				if (target.TryGetComponent<LivingEntity>(out var targetScript))
				{
					targetScript.TakeDamage(10);
					targetScript.GetKnockedBack(this.transform.position, knockBack);
				}
			}
		}

		private Collider2D[] DetectHostilesInRange(float range)
		{
			return Utility.DetectByLayers(attackPoint.position, range, hostileLayers);
		}
	}
}