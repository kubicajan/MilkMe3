using System.Collections;
using UnityEngine;

public class Archer : PersonaAbstract
{
	private const float RANGE_ATTACK_DISTANCE = 8f;
	[SerializeField] private LineRenderer laser;

	[SerializeField] private GameObject arrowPrefab;
	public override string PersonaName { get; set; } = "Archer";

	public override void BaseAttack()
	{
		StartCoroutine(RangeAttack());
	}

	private IEnumerator RangeAttack()
	{
		Transform playerAttackPoint = playerBase.attackPoint;
		RaycastHit2D enemyHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
			RANGE_ATTACK_DISTANCE, playerBase.enemyLayers);
		RaycastHit2D groundHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
			RANGE_ATTACK_DISTANCE, playerBase.groundLayers);
		GameObject projectile = new GameObject();
		projectile.SetActive(false);

		if (enemyHitInfo)
		{
			EnemyScript enemyScript = enemyHitInfo.transform.GetComponent<EnemyScript>();
			if (enemyScript != null)
			{
				enemyScript.TakeDamage(10);
				enemyScript.GetKnockedBack(this.transform.position, 0.5f);
				Utility.SetLaserPosition(laser, playerAttackPoint.position, enemyHitInfo.point);
				projectile = Instantiate(arrowPrefab, enemyHitInfo.point, playerAttackPoint.rotation);
				projectile.transform.parent = enemyHitInfo.transform;
			}
		}
		else if (groundHitInfo)
		{
			Utility.SetLaserPosition(laser, playerAttackPoint.position, groundHitInfo.point);
			projectile = Instantiate(arrowPrefab, groundHitInfo.point, playerAttackPoint.rotation);
		}
		else
		{
			Vector2 secondPosition = new Vector2((lastDirection * RANGE_ATTACK_DISTANCE) + playerAttackPoint.position.x,
				playerAttackPoint.position.y);
			Utility.SetLaserPosition(laser, playerAttackPoint.position, secondPosition);
		}

		laser.enabled = true;
		projectile.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		laser.enabled = false;
	}

	public override void FirstAbility()
	{
		Debug.Log("Unfinished");
		return;
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