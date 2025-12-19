using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Helpers;
using Helpers.CommonEnums;
using Living.Enemies.FarmerBoss;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Living.Enemies.FarmerBoss
{
	public class FarmerBoss : EnemyScript
	{
		[SerializeField] private Animator animator;
		[SerializeField] public GameObject pitchfork;
		[SerializeField] private Transform pitchforkAttackPoint;
		[SerializeField] private LineRenderer laser;

		private void Awake()
		{
			movementSpeed = 5f;
			gameObject.tag = GameTag.Npc;
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		float timer = 0f;
		const float duration = 4f;

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			timer += Time.fixedDeltaTime;

			if (!isAttacking && CanAttack() && timer >= duration)
			{
				Transform pitchfork = transform.Find("RightArm/Pitchfork");

				pitchfork.gameObject.SetActive(true);

				timer = 0f; // reset if needed
				animator.SetTrigger(FarmerBossTrigger.ThrowPitchfork);
			}
		}

		private bool CanAttack()
		{
			return gameObject.CompareTag(GameTag.Boss);
		}

		private void ThrowPitchfork()
		{
			StartCoroutine(PitchforkThrow());
		}

		private IEnumerator PitchforkThrow()
		{
			//TODO: CREATE THROWABLE PITCHFORK THAT HAS EFFECTS AND SHIT
			// Instantiate(pitchfork, attackPoint.position, Quaternion.identity);

			Transform pitchfork = transform.Find("RightArm/Pitchfork");
			 pitchfork.gameObject.SetActive(false);

			Vector2 secondPosition = new Vector2(100 + pitchforkAttackPoint.position.x,
				100 + pitchforkAttackPoint.position.y);
			Utility.SetLaserPosition(laser, pitchforkAttackPoint.position, secondPosition);

			laser.enabled = true;
			yield return new WaitForSeconds(0.05f);
			// laser.enabled = false;
		}


		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			// GetComponent<Animator>().SetTrigger(WarriorBossTrigger.Annoyed);
			GetComponent<Animator>().SetTrigger(FarmerBossTrigger.Annoyed);

			gameObject.tag = GameTag.Boss;
		}
	}
}