using Persona.Blueprints;
using UnityEngine;

namespace Persona
{
	public class Mage : PersonaAbstract
	{
		[SerializeField] private ParticleSystem shieldParticleEffect;
		[SerializeField] private GameObject tornadoPrefab;

		public override string PersonaName { get; set; } = "Mage";

		public override void BaseAttack()
		{
			Debug.Log("Unfinished");
			return;
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

		private void ShootTornado()
		{
			Instantiate(tornadoPrefab, transform.position, gameObject.transform.rotation);
		}

		private void ActivateShield()
		{
			shieldParticleEffect.Play();
			shieldParticleEffect.GetComponent<CircleCollider2D>().enabled = true;
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