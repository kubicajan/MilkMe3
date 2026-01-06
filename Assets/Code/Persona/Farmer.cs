using System.Collections;
using Code.Persona.Blueprints;
using Helpers;
using Helpers.CommonEnums;
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
			const float RANGE_ATTACK_DISTANCE = 25f;

			chargingParticleEffect.Play();
			yield return new WaitForSeconds(1.5f);
			chargingParticleEffect.Stop();

			yield return new WaitForSeconds(0.5f);
			Transform playerAttackPoint = playerBase.attackPoint;

			LayerMask hitLayers = playerBase.hostileLayers | playerBase.groundLayers;
			RaycastHit2D hit = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
				RANGE_ATTACK_DISTANCE, hitLayers);


			if (hit.collider == null)
			{
				Vector2 secondPosition = new Vector2(
					(lastDirection * RANGE_ATTACK_DISTANCE) + playerAttackPoint.position.x,
					playerAttackPoint.position.y);
				Utility.SetLaserPosition(laser, playerAttackPoint.position, secondPosition);
			}
			else if (hit.collider.CompareTag(GameTag.Boss) || hit.collider.CompareTag(GameTag.Enemy))
			{
				EnemyScript enemy = hit.collider.GetComponent<EnemyScript>();
				Utility.SetLaserPosition(laser, playerAttackPoint.position, hit.point);
				GameObject projectile = Instantiate(pitchForkPrefab, hit.point, GetRotation());
				projectile.transform.parent = hit.transform;

				if (enemy != null)
				{
					enemy.TakeDamage(10);
					enemy.MagicPushMe(hit.point, 5);
				}
			}
			else if (hit.collider.CompareTag(GameTag.Ground))
			{
				Utility.SetLaserPosition(laser, playerAttackPoint.position, hit.point);
				Instantiate(pitchForkPrefab, hit.point, GetRotation());
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
	}
}