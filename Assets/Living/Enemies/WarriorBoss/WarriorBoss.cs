using System.Collections;
using System.Linq;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBoss : EnemyScript
	{
		[SerializeField] private Transform attackPoint;
		[SerializeField] private ParticleSystem highlightParticleEffect;
		[SerializeField] public GameObject heavyRangeAttack;
		[SerializeField] private Animator animator;
		private const float MELEE_ATTACK_RANGE = 3f;

		private void Awake()
		{
			movementSpeed = 9f;
			gameObject.tag = GameTag.Npc;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!isAttacking && CanAttack())
			{
				animator.SetTrigger(SelectAttack());
			}
		}

		public void HeavyAttack()
		{
			const float KNOCKBACK = 20;
			MeleeAttack(KNOCKBACK);
			// Instantiate(heavyRangeAttack, transform.position, Quaternion.identity);
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
			StartCoroutine(ItemManager.Instance.SpawnItems(7, transform.position));
			gameObject.layer = LayerMask.NameToLayer(GameLayer.Prop);
			gameObject.tag = GameTag.Prop;
			StartCoroutine(CreateGodBeams());
			//TODO:THIS WHOLE THING SHOULD BE REPLACED WITH AN IMAGE
			GetComponent<Rigidbody2D>().simulated = false;
			// StartCoroutine(DieCoroutine(5));
		}

		private IEnumerator CreateGodBeams()
		{
			yield return new WaitForSeconds(1.5f);
			highlightParticleEffect.Play();
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Annoyed);
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