using System.Collections;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Living.Enemies.FarmerBoss
{
	public class FarmerBoss : EnemyScript
	{
		[SerializeField] private Animator animator;
		[SerializeField] private GameObject pitchforkInHand;
		[SerializeField] private GameObject pitchforkFallPrefab;
		[SerializeField] private Transform pitchforkAttackPoint;
		[SerializeField] private LineRenderer laser;
		private GameObject pitchforkFallCopy;

		private bool hasPitchfork = true;
		private Vector2 pitchforkLocation;

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

		public void PickupPitchfork()
		{
			SetHasPitchfork(true);
			pitchforkLocation = Vector2.negativeInfinity;
			MakePitchforkVisible();
			Destroy(pitchforkFallCopy);
			targetPosition = playerLocation.position;
		}

		private bool CanAttack()
		{
			return gameObject.CompareTag(GameTag.Boss) && hasPitchfork;
		}

		private void ThrowPitchfork()
		{
			StartCoroutine(PitchforkThrowCoroutine());
		}

		private IEnumerator PitchforkThrowCoroutine()
		{
			pitchforkInHand.gameObject.SetActive(false);

			Vector2 secondPosition = new Vector2(pitchforkAttackPoint.position.x + 100 * lastDirection,
				100 + pitchforkAttackPoint.position.y);
			Utility.SetLaserPosition(laser, pitchforkAttackPoint.position, secondPosition);

			pitchforkLocation = playerLocation.position;
			pitchforkFallCopy = Instantiate(pitchforkFallPrefab, pitchforkLocation, Quaternion.identity);
			pitchforkFallCopy.gameObject.SetActive(true);
			laser.enabled = true;
			yield return new WaitForSeconds(0.5f);
			laser.enabled = false;
			targetPosition = pitchforkLocation;
		}

		public void SetHasPitchfork(bool hasIt)
		{
			hasPitchfork = hasIt;
		}

		public void MakePitchforkVisible()
		{
			pitchforkInHand.gameObject.SetActive(true);
		}

		public void MakePitchforkInvisible()
		{
			pitchforkInHand.gameObject.SetActive(false);
		}

		private bool IsNear(Vector2 position)
		{
			double tolerance = 2.5;
			double toleranceSqr = tolerance * tolerance;

			return ((Vector2)transform.position - position).sqrMagnitude <= toleranceSqr;
		}

		public override Vector2 GetMoveDestination()
		{
			if (!hasPitchfork && pitchforkLocation != Vector2.negativeInfinity)
			{
				return new Vector3(pitchforkLocation.x, transform.position.y);
			}
			else
			{
				return new Vector3(playerLocation.position.x, transform.position.y);
			}
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(FarmerBossTrigger.Annoyed);
			gameObject.tag = GameTag.Boss;
		}
	}
}