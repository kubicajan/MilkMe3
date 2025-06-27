using System.Collections;
using Helpers;
using Persona.Blueprints;
using UnityEngine;

namespace Persona
{
	public class Warrior : PersonaAbstract
	{
		[SerializeField] private ParticleSystem stompParticle;

		private const float MELEE_ATTACK_RANGE = 2f;
		private int attackCounter = 0;
		private bool canAttack = true;
		protected override int MaxNumberOfJumps => 1;

		public override string PersonaName { get; set; } = "Warrior";

		public override void BaseAttack()
		{
			MeleeAttack();
		}

		public override void FirstAbility()
		{
			KickAttack();
		}

		public override void SecondAbility()
		{
			LiftAttack();
		}

		public override void SwapToMe()
		{
			Debug.Log("Unfinished");
			return;
		}

		public override void SwapFromMe()
		{
			Debug.Log("Unfinished");
			return;
		}


		//todo: jako to funguje, ale ne uplne. musel bych vypnout vsechnu gravitaci pro ty debily a movement
		//protected override IEnumerator DashCoroutine()
		//{
		//    ResetJumps();
		//    Common.TurnOffGravity(RigidBody, true);
		//    float distanceToDash = lastDirection * 30;
		//    RigidBody.velocity = new Vector2(distanceToDash, 0);
		//    Collider2D[] detectedEnemies = DetectEnemiesInRange(distanceToDash);
		//    ProcessEnemies(detectedEnemies, enemyScript => enemyScript.Immobilize(true));
		//    yield return new WaitForSeconds(0.3f);
		//    RigidBody.velocity = Vector2.zero;
		//    yield return new WaitForSeconds(0.1f);
		//    Common.TurnOffGravity(RigidBody, false);
		//}


		private void MeleeAttack()
		{
			if (canAttack)
			{
				StartCoroutine(MakeAttackGoOnCooldown());
				const float KNOCKBACK = 0.2f;
				const int MOVE_BY = 2;
				attackCounter++;

				if (attackCounter >= 3)
				{
					attackCounter = 0;
					StompAttack();
				}
				else
				{
					Collider2D[] detectedEnemies = DetectEnemiesInRange(MELEE_ATTACK_RANGE);
					DealDamageTo(detectedEnemies, KNOCKBACK);
					AttackMoveAllEnemiesHit(detectedEnemies, MOVE_BY);
					MoveAttackMeBy(MOVE_BY);
				}
			}
		}

		private void KickAttack()
		{
			Collider2D[] detectedEnemies = DetectEnemiesInRange(MELEE_ATTACK_RANGE);
			DealDamageTo(detectedEnemies, 0);
			ProcessEnemies(detectedEnemies, enemyScript => enemyScript.MagicPushMe(transform.position, 10));
		}

		private IEnumerator MakeAttackGoOnCooldown()
		{
			canAttack = false;
			yield return new WaitForSeconds(0.25f);
			canAttack = true;
		}

		private void MoveAttackMeBy(int moveBy)
		{
			RunMovementCoroutine(Common.WarriorMoveAttack(transform.position.x, moveBy, lastDirection, transform,
				RigidBody, null));
		}

		private void LiftAttack()
		{
			const int LIFT_BY_THIS_MUCH = 5;
			Collider2D[] detectedEnemies = DetectEnemiesInRange(MELEE_ATTACK_RANGE);
			LiftUpAllEnemiesHit(detectedEnemies, LIFT_BY_THIS_MUCH);
			LiftMeUpBy(LIFT_BY_THIS_MUCH);
		}

		private void LiftMeUpBy(int liftByThisMuch)
		{
			RunMovementCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform, null));
		}

		private void StompAttack()
		{
			const float AREA_OF_EFFECT = 5f;
			const float KNOCKBACK = 2;
			const int STOMP_SPEED = -100;

			if (!IsGrounded())
			{
				StompDownAllEnemiesHit(DetectEnemiesInRange(MELEE_ATTACK_RANGE), STOMP_SPEED);
				StartCoroutine(StompDown(AREA_OF_EFFECT, STOMP_SPEED, KNOCKBACK));
			}
			else
			{
				Instantiate(stompParticle, transform.position, Quaternion.identity);
				DealDamageTo(DetectEnemiesInRange(AREA_OF_EFFECT), KNOCKBACK);
			}
		}

		private IEnumerator StompDown(float areaOfEffect, int stompSpeed, float landingKnockBack)
		{
			Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.hostileLayers);

			while (!IsGrounded())
			{
				RigidBody.velocity = new Vector2(0, stompSpeed);
				yield return null;
			}

			RigidBody.velocity = Vector2.zero;
			yield return new WaitForSeconds(0.05f);
			Instantiate(stompParticle, transform.position, Quaternion.identity);
			DealDamageTo(DetectEnemiesInRange(areaOfEffect), landingKnockBack);
			Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.hostileLayers);
		}

		private void AttackMoveAllEnemiesHit(Collider2D[] detectedEntities, int moveBy)
		{
			ProcessEnemies(detectedEntities, enemyScript => enemyScript.AttackMoveMe(moveBy, lastDirection));
		}

		private void LiftUpAllEnemiesHit(Collider2D[] detectedEntities, int liftByThisMuch)
		{
			ProcessEnemies(detectedEntities, enemyScript => enemyScript.LiftMeUp(liftByThisMuch));
		}

		private void StompDownAllEnemiesHit(Collider2D[] detectedEntities, int stompSpeed)
		{
			ProcessEnemies(detectedEntities, enemyScript => enemyScript.StompMeDown(stompSpeed));
		}

		void OnDrawGizmosSelected()
		{
			if (playerBase.attackPoint.position == null)
			{
				return;
			}

			Gizmos.DrawWireSphere(playerBase.attackPoint.position, MELEE_ATTACK_RANGE);
		}
	}
}