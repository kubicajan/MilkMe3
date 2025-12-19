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

namespace Living.Enemies.WarriorBoss
{
	public class FarmerBoss : EnemyScript
	{
		[SerializeField] private Animator animator;

		private void Awake()
		{
			movementSpeed = 5f;
			gameObject.tag = GameTag.Npc;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			float timer = 0f;
			const float duration = 2f;
			timer += Time.fixedDeltaTime;

			if (!isAttacking && CanAttack() )
			{
				animator.SetTrigger(ThrowPitchfork());
				timer = 0f; // reset if needed
			}
		}

		private bool CanAttack()
		{
			return true;
		}

		private string ThrowPitchfork()
		{
			return FarmerBossTrigger.ThrowPitchfork;
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