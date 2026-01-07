using System.Collections;
using Code.Living.Player;
using Code.Persona.Blueprints;
using UnityEngine;

namespace Code.Persona
{
	public class Archer : PersonaAbstract
	{
		[SerializeField] private LineRenderer laser;
		[SerializeField] private GameObject arrowStrikePrefab;
		[SerializeField] private ArrowScript arrowPrefab;
		[SerializeField] private GameObject bulletPrefab;
		private bool gunslinger = false;

		public override string PersonaName { get; set; } = "Archer";

		public override void BaseAttack()
		{
			// StartCoroutine(RangeAttack());
			var newArrow = Instantiate(arrowPrefab, playerBase.attackPoint.position, Quaternion.identity);
			newArrow.Instantiate(lastDirection);
		}

		public override void Initialize(PlayerBase _playerBase)
		{
			base.Initialize(_playerBase);
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		// private IEnumerator RangeAttack()
		// {
		// 	Transform playerAttackPoint = playerBase.attackPoint;
		// 	if (gunslinger)
		// 	{
		// 		Instantiate(bulletPrefab, transform.position, GetRotation());
		// 		yield break;
		// 	}
		//
		// 	RaycastHit2D enemyHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
		// 		RANGE_ATTACK_DISTANCE, playerBase.hostileLayers);
		// 	RaycastHit2D groundHitInfo = Physics2D.Raycast(playerBase.attackPoint.position,
		// 		playerBase.attackPoint.right,
		// 		RANGE_ATTACK_DISTANCE, playerBase.groundLayers);
		// 	RaycastHit2D npcHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
		// 		RANGE_ATTACK_DISTANCE, playerBase.npcLayers);
		//
		// 	if (enemyHitInfo)
		// 	{
		// 		EnemyScript enemyScript = enemyHitInfo.transform.GetComponent<EnemyScript>();
		// 		if (enemyScript != null)
		// 		{
		// 			enemyScript.TakeDamage(10);
		// 			enemyScript.GetKnockedBack(this.transform.position, 0.5f);
		// 			Utility.SetLaserPosition(laser, playerAttackPoint.position, enemyHitInfo.point);
		// 			GameObject projectile = Instantiate(ArrowStrikePrefab, enemyHitInfo.point, GetRotation());
		// 			projectile.transform.parent = enemyHitInfo.transform;
		// 		}
		// 	}
		// 	else if (npcHitInfo)
		// 	{
		// 		Utility.SetLaserPosition(laser, playerAttackPoint.position, npcHitInfo.point);
		// 		GameObject projectile = Instantiate(ArrowStrikePrefab, npcHitInfo.point, GetRotation());
		// 		projectile.transform.parent = npcHitInfo.transform;
		// 	}
		// 	else if (groundHitInfo)
		// 	{
		// 		Utility.SetLaserPosition(laser, playerAttackPoint.position, groundHitInfo.point);
		// 		Instantiate(ArrowStrikePrefab, groundHitInfo.point, GetRotation());
		// 	}
		// 	else
		// 	{
		// 		Vector2 secondPosition = new Vector2(
		// 			(lastDirection * RANGE_ATTACK_DISTANCE) + playerAttackPoint.position.x,
		// 			playerAttackPoint.position.y);
		// 		Utility.SetLaserPosition(laser, playerAttackPoint.position, secondPosition);
		// 	}
		//
		// 	laser.enabled = true;
		// 	yield return new WaitForSeconds(0.05f);
		// 	laser.enabled = false;
		// }

		public override void FirstAbility()
		{


			StartCoroutine(SpawnTopHalfCircle());
		}

		IEnumerator SpawnTopHalfCircle()
		{
			int spawnCount = 20;
			float radius = 3f;
			float minDistance = 0.4f;

			Vector2 lastSpawnPos = Vector2.zero;

			for (int i = 0; i < spawnCount; i++)
			{
				Vector2 spawnPos;
				int attempts = 0;
				do
				{
					float minAngle = 20f * Mathf.Deg2Rad; // Convert 20 degrees to radians
					float maxAngle = 160f * Mathf.Deg2Rad; // Convert 160 degrees to radians

					float angle = Random.Range(minAngle, maxAngle);
					Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
					spawnPos = (Vector2)transform.position + offset;
					attempts++; // Safety check to avoid infinite loop
					if (attempts > 20) break;
				} while (i > 0 && Vector2.Distance(spawnPos, lastSpawnPos) < minDistance);

				// Direction toward the center
				Vector2 direction = (Vector2)transform.position - spawnPos;
				float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				Quaternion rotation = Quaternion.Euler(0f, 0f, angleDeg);
				GameObject newArrow = Instantiate(arrowStrikePrefab, spawnPos, rotation);
				newArrow.transform.parent = gameObject.transform;
				lastSpawnPos = spawnPos;
				yield return new WaitForSeconds(0.15f);
			}
		}

		public override void SecondAbility()
		{
			Debug.Log("Unfinished");
			return;
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
	}
}