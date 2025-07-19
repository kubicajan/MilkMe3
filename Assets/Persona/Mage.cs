using Persona.Blueprints;
using UnityEngine;

namespace Persona
{
	public class Mage : PersonaAbstract
	{
		[SerializeField] private ParticleSystem shieldParticleEffect;
		[SerializeField] private GameObject tornadoPrefab;
		[SerializeField] private GameObject lightningPrefab;
		private const int SHIELD_HEALTH = 50;
		public override string PersonaName { get; set; } = "Mage";

		public override void BaseAttack()
		{
			LightningAttack();
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

		private void LightningAttack()
		{
			Collider2D[] enemiesInRange = DetectEnemiesInRange(100);
			DealDamageTo(enemiesInRange, 10);

			foreach (Collider2D enemy in enemiesInRange)
			{
				Instantiate(lightningPrefab, enemy.transform.position, gameObject.transform.rotation);
			}
		}

		private void ShootTornado()
		{
			Instantiate(tornadoPrefab, transform.position, gameObject.transform.rotation);
		}

		private void ActivateShield()
		{
			playerBase.ActivateShield(SHIELD_HEALTH, shieldParticleEffect, shieldParticleEffect.GetComponent<CircleCollider2D>());
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