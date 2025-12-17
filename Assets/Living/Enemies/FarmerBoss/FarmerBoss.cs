using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Helpers;
using Helpers.CommonEnums;
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