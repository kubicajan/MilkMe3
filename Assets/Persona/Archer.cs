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
		RaycastHit2D enemyHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
			RANGE_ATTACK_DISTANCE, playerBase.enemyLayers);
		RaycastHit2D groundHitInfo = Physics2D.Raycast(playerBase.attackPoint.position, playerBase.attackPoint.right,
			RANGE_ATTACK_DISTANCE, playerBase.groundLayers);

		if (enemyHitInfo)
		{
			EnemyScript enemyScript = enemyHitInfo.transform.GetComponent<EnemyScript>();
			if (enemyScript != null)
			{
				Debug.Log("hit enemy");
				enemyScript.TakeDamage(10);
				enemyScript.GetKnockedBack(this.transform.position, 0.5f);
				laser.SetPosition(0, playerBase.attackPoint.position);
				laser.SetPosition(1, enemyHitInfo.point);
				GameObject arrow = Instantiate(arrowPrefab, enemyHitInfo.point, playerBase.attackPoint.rotation);
				arrow.transform.parent = enemyHitInfo.transform;
			}
		}
		else if (groundHitInfo)
		{
			laser.SetPosition(0, playerBase.attackPoint.position);
			laser.SetPosition(1, groundHitInfo.point);
			Instantiate(arrowPrefab, groundHitInfo.point, playerBase.attackPoint.rotation);
		}
		else
		{
			laser.SetPosition(0, playerBase.attackPoint.position);
			laser.SetPosition(1,
				new Vector2((lastDirection * RANGE_ATTACK_DISTANCE) + playerBase.attackPoint.position.x,
					playerBase.attackPoint.position.y));
		}

		laser.enabled = true;
		yield return new WaitForSeconds(0.1f);
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