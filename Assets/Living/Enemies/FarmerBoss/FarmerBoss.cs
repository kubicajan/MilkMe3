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

		private float timer = 0f;
		const float duration = 4f;

		private void Awake()
		{
			movementSpeed = 5f;
			gameObject.tag = GameTag.Npc;
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			timer += Time.fixedDeltaTime;

			if (!hasPitchfork && IsNear(pitchforkLocation))
			{
				animator.SetTrigger(FarmerBossTrigger.PickUpPitchfork);
				Debug.Log("hello");
			}
			else if (!isAttacking && CanAttack() && timer >= duration)
			{
				Transform pitchfork = transform.Find("RightArm/Pitchfork");
				pitchfork.gameObject.SetActive(true);
				timer = 0f;
				animator.SetTrigger(FarmerBossTrigger.ThrowPitchfork);
				hasPitchfork = false;
			}
		}

		private bool IsNear(Vector2 targetPosition)
		{
			double tolerance = 2;
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
				if (hasPitchfork)
				{
					Vector3 targetPosition =
						new Vector3(playerLocation.position.x, transform.position.y, transform.position.z);
					transform.position =
						Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
				}
				else
				{
					Vector3 targetPosition =
						new Vector3(pitchforkLocation.x, transform.position.y, transform.position.z);
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
			yield return new WaitForSeconds(0.5f);
			laser.enabled = false;

			pitchforkLocation = playerLocation.position;
			Instantiate(pitchforkFall, pitchforkLocation, Quaternion.identity);
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(FarmerBossTrigger.Annoyed);
			gameObject.tag = GameTag.Boss;
		}
	}
}