using System.Collections;
using Helpers;
using Living.Enemies;
using Persona.Blueprints;
using UnityEngine;

namespace Persona
{
	public class Farmer : PersonaAbstract
	{
		[SerializeField] private ParticleSystem angryParticleEffect;
		[SerializeField] private ParticleSystem chargingParticleEffect;
		[SerializeField] private LineRenderer laser;
		[SerializeField] private GameObject pitchForkPrefab;

		private const float MELEE_ATTACK_RANGE = 2f;
		private bool isAngry = false;

		public override string PersonaName { get; set; } = "Farmer";

		public override void BaseAttack()
		{
			MeleeAttack();
		}

		public override void FirstAbility()
		{
			if (!isAngry)
			{
				MakeAngry();
			}
			else
			{
				CalmDown();
			}
		}

		public override void SecondAbility()
		{
			StartCoroutine(PitchforkThrow());
		}

		private IEnumerator PitchforkThrow()
		{
			chargingParticleEffect.Play();

			//charge it
			yield return new WaitForSeconds(1.5f);
			chargingParticleEffect.Stop();
			yield return new WaitForSeconds(0.5f);
			const float RANGE_ATTACK_DISTANCE = 25f;
			Transform playerAttackPoint = playerBase.attackPoint;
			RaycastHit2D enemyHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
				RANGE_ATTACK_DISTANCE, playerBase.hostileLayers);
			RaycastHit2D groundHitInfo = Physics2D.Raycast(playerBase.attackPoint.position,
				playerBase.attackPoint.right,
				RANGE_ATTACK_DISTANCE, playerBase.groundLayers);


			if (enemyHitInfo)
			{
				EnemyScript enemyScript = enemyHitInfo.transform.GetComponent<EnemyScript>();
				if (enemyScript != null)
				{
					Utility.SetLaserPosition(laser, playerAttackPoint.position, enemyHitInfo.point);
					GameObject projectile = Instantiate(pitchForkPrefab, enemyHitInfo.point, GetRotation());
					projectile.transform.parent = enemyHitInfo.transform;
					enemyScript.TakeDamage(10);
					enemyScript.MagicPushMe(enemyHitInfo.point, 5);
				}
			}
			else if (groundHitInfo)
			{
				Utility.SetLaserPosition(laser, playerAttackPoint.position, groundHitInfo.point);
				Instantiate(pitchForkPrefab, groundHitInfo.point, GetRotation());
			}
			else
			{
				Vector2 secondPosition = new Vector2(
					(lastDirection * RANGE_ATTACK_DISTANCE) + playerAttackPoint.position.x,
					playerAttackPoint.position.y);
				Utility.SetLaserPosition(laser, playerAttackPoint.position, secondPosition);
			}

			laser.enabled = true;
			yield return new WaitForSeconds(0.05f);
			laser.enabled = false;
		}

		public override void SwapToMe()
		{
			Debug.Log("Unfinished");
			return;
		}

		public override void SwapFromMe()
		{
			CalmDown();
		}

		private void CalmDown()
		{
			isAngry = false;
			angryParticleEffect.Stop();
		}

		private void MakeAngry()
		{
			isAngry = true;
			angryParticleEffect.Play();
		}

		private void MeleeAttack()
		{
			const float KNOCKBACK = 2;
			DealDamageTo(DetectEnemiesInRange(MELEE_ATTACK_RANGE), KNOCKBACK);
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