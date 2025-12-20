using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using Helpers;
using Helpers.CommonEnums;
using Living.Enemies.FarmerBoss;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Living.Enemies.FarmerBoss
{
	public class FarmerBoss : EnemyScript
	{
		[SerializeField] private Animator animator;
		[SerializeField] public GameObject pitchfork;
		[SerializeField] public GameObject pitchforkFall;
		[SerializeField] private Transform pitchforkAttackPoint;
		[SerializeField] private LineRenderer laser;
		private bool hasPitchfork = true;
		private Vector2 pitchforkLocation;

		public GameObject pitchforkFallCopy;

		private void Awake()
		{
			movementSpeed = 5f;
			gameObject.tag = GameTag.Npc;
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (!hasPitchfork && IsNear(pitchforkLocation))
			{
				animator.SetTrigger(FarmerBossTrigger.PickUpPitchfork);
			}
			else if (!isAttacking && CanAttack())
			{
				animator.SetTrigger(FarmerBossTrigger.ThrowPitchfork);
			}
		}

		public void MakePitchforkVisible()
		{
			pitchfork.gameObject.SetActive(true);
		}

		public void MakePitchforkInvisible()
		{
			pitchfork.gameObject.SetActive(false);
		}

		public void PickupPitchfork()
		{
			hasPitchfork = true;
			pitchforkLocation = Vector2.negativeInfinity;
			MakePitchforkVisible();
			Destroy(pitchforkFallCopy);
		}

		private bool IsNear(Vector2 targetPosition)
		{
			double tolerance = 2.5;
			double toleranceSqr = tolerance * tolerance;

			return ((Vector2)transform.position - targetPosition).sqrMagnitude <= toleranceSqr;
		}

		private bool CanAttack()
		{
			return gameObject.CompareTag(GameTag.Boss) && hasPitchfork;
		}

		private void ThrowPitchfork()
		{
			StartCoroutine(PitchforkThrow());
		}

		public override void Move()
		{
			if (!IsImmobilized())
			{
				if (!hasPitchfork && pitchforkLocation != Vector2.negativeInfinity)
				{
					Vector3 targetPosition =
						new Vector3(pitchforkLocation.x, transform.position.y, transform.position.z);
					transform.position =
						Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
				}
				else
				{
					Vector3 targetPosition =
						new Vector3(playerLocation.position.x, transform.position.y, transform.position.z);
					transform.position =
						Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
				}
			}
			else
			{
				if (IsGrounded() && RigidBody.velocity == Vector2.zero)
				{
					Immobilize(false);
				}
			}
		}

		private IEnumerator PitchforkThrow()
		{
			Transform pitchfork = transform.Find("RightArm/Pitchfork");
			pitchfork.gameObject.SetActive(false);

			Vector2 secondPosition = new Vector2(pitchforkAttackPoint.position.x + 100 * lastDirection,
				100 + pitchforkAttackPoint.position.y);
			Utility.SetLaserPosition(laser, pitchforkAttackPoint.position, secondPosition);

			laser.enabled = true;

			pitchforkLocation = playerLocation.position;
			pitchforkFallCopy = Instantiate(pitchforkFall, pitchforkLocation, Quaternion.identity);
			pitchforkFallCopy.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.5f);
			laser.enabled = false;
		}

		public void SetHasPitchfork(bool hasIt)
		{
			this.hasPitchfork = hasIt;
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(FarmerBossTrigger.Annoyed);
			gameObject.tag = GameTag.Boss;
		}
	}
}