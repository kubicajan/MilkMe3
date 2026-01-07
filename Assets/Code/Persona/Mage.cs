using System.Collections;
using System.Linq;
using Code.Persona.Blueprints;
using UnityEngine;

namespace Code.Persona
{
	public class Mage : PersonaAbstract
	{
		[SerializeField] private ParticleSystem shieldParticleEffect;
		[SerializeField] private TornadoScript tornadoPrefab;
		[SerializeField] private Lightning lightningPrefab;
		private const int SHIELD_HEALTH = 50;
		public override string PersonaName { get; set; } = "Mage";

		public override void BaseAttack()
		{
			StartCoroutine(LightningAttack());
		}

		public override void FirstAbility()
		{
			ShootTornado();
		}

		public override void SecondAbility()
		{
			MagicPushBack();
		}

		public override void SwapToMe()
		{
			ActivateShield();
		}

		public override void SwapFromMe()
		{
			DeactivateShield();
		}

		private IEnumerator LightningAttack()
		{
			Collider2D[] enemiesInRange = DetectEnemiesInRange(10);
			DealDamageTo(enemiesInRange, 0);
			var shuffledEnemies = enemiesInRange.OrderBy(_ => new System.Random().Next()).ToArray();

			foreach (Collider2D enemy in shuffledEnemies)
			{
				Lightning lightning = Instantiate(lightningPrefab, new Vector2(), Quaternion.identity);
				lightning.CreateLightning(enemy.transform);
				yield return new WaitForSeconds(0.05f);
			}
		}

		private void ShootTornado()
		{
			TornadoScript.Initialize(tornadoPrefab, transform.position, Quaternion.identity, lastDirection);
		}

		private void ActivateShield()
		{
			playerBase.ActivateShield(SHIELD_HEALTH, shieldParticleEffect,
				shieldParticleEffect.GetComponent<CircleCollider2D>());
		}

		private void DeactivateShield()
		{
			shieldParticleEffect.Stop();
			shieldParticleEffect.GetComponent<CircleCollider2D>().enabled = false;
		}

		private void MagicPushBack()
		{
			const float DISTANCE_LIMIT = 10f;
			MagicPushAllEnemies(DetectEnemiesInRange(DISTANCE_LIMIT / 2), transform.position, DISTANCE_LIMIT);
		}

		private void MagicPushAllEnemies(Collider2D[] detectedEntities, Vector2 perpetratorPosition,
			float distanceLimit)
		{
			ProcessEnemies(detectedEntities,
				enemyScript => enemyScript.MagicPushMe(perpetratorPosition, distanceLimit));
		}
	}
}