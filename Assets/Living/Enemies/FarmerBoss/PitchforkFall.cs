using System.Collections;
using Helpers;
using UnityEngine;

namespace Living.Enemies.FarmerBoss
{
	public class PitchforkFall : MonoBehaviour
	{
		[SerializeField] private LineRenderer laser;

		private void Awake()
		{
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		private IEnumerator SpawnLightningFromTop()
		{
			Vector2 secondPosition = new Vector2(transform.position.x + 100,
				100 + transform.position.y);
			Utility.SetLaserPosition(laser, transform.position, secondPosition);

			laser.enabled = true;
			yield return new WaitForSeconds(0.05f);
			laser.enabled = false;
		}

		private void LightningFromTop()
		{
			StartCoroutine(SpawnLightningFromTop());
		}
	}
}