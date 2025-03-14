using System.Collections;
using Living.Enemies;
using Persona.Blueprints;
using UnityEngine;

namespace Persona
{
	public class Farmer : PersonaAbstract
	{
		[SerializeField] private ParticleSystem angryParticleEffect;
		[SerializeField] private LineRenderer laser;
		[SerializeField] private GameObject pitchForkPrefab;

		private const float MEELE_ATTACK_RANGE = 2f;
		private bool isAngry = false;

		public override string PersonaName { get; set; } = "Farmer";

		public override void BaseAttack()
		{
			MeeleAttack();
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
			//charge it
			yield return new WaitForSeconds(1f);
			const float RANGE_ATTACK_DISTANCE = 25f;
			Transform playerAttackPoint = playerBase.attackPoint;
			RaycastHit2D enemyHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
				RANGE_ATTACK_DISTANCE, playerBase.enemyLayers);
			RaycastHit2D groundHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
				RANGE_ATTACK_DISTANCE, playerBase.groundLayers);


			if (enemyHitInfo)
			{
				EnemyScript enemyScript = enemyHitInfo.transform.GetComponent<EnemyScript>();
				if (enemyScript != null)
				{
					Utility.SetLaserPosition(laser, playerAttackPoint.position, enemyHitInfo.point);
					GameObject projectile = Instantiate(pitchForkPrefab, enemyHitInfo.point, playerAttackPoint.rotation);
					projectile.transform.parent = enemyHitInfo.transform;
					enemyScript.TakeDamage(10);
					enemyScript.MagicPushMe(enemyHitInfo.point, 5);
				}
			}
			else if (groundHitInfo)
			{
				Utility.SetLaserPosition(laser, playerAttackPoint.position, groundHitInfo.point);
				Instantiate(pitchForkPrefab, groundHitInfo.point, playerAttackPoint.rotation);
			}
			else
			{
				Vector2 secondPosition = new Vector2((lastDirection * RANGE_ATTACK_DISTANCE) + playerAttackPoint.position.x,
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

		private void MeeleAttack()
		{
			const float KNOCKBACK = 2;
			DealDamageTo(DetectEnemiesInRange(MEELE_ATTACK_RANGE), KNOCKBACK);
		}

		void OnDrawGizmosSelected()
		{
			if (playerBase.attackPoint.position == null)
			{
				return;
			}

			Gizmos.DrawWireSphere(playerBase.attackPoint.position, MEELE_ATTACK_RANGE);
		}
	}
}