using System.Collections;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

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
		private Transform pitchforkLocation;

		private void Awake()
		{
			pitchforkLocation = new GameObject("PitchforkLocation").transform;
			movementSpeed = 5f;
			gameObject.tag = GameTag.Npc;
			laser.GetComponent<LineRenderer>().positionCount = 2;
		}

		public void FixedUpdate()
		{
			if (!hasPitchfork && IsNear(pitchforkLocation.position))
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
			Destroy(pitchforkFallCopy);
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

			pitchforkLocation.position = Utility.GetGroundBelowLocation(playerLocation.position);
			pitchforkFallCopy = Instantiate(pitchforkFallPrefab, pitchforkLocation.position, Quaternion.identity);
			pitchforkFallCopy.gameObject.SetActive(true);
			laser.enabled = true;
			yield return new WaitForSeconds(0.5f);
			laser.enabled = false;
		}

		public void DoJumpAttack()
		{
			SetPlayerAsTarget();
			Vector2 startPos = transform.position;
			Vector2 targetPos = Utility.GetGroundBelowLocation(targetLocation.position);

			StartCoroutine(JumpRoutine(startPos, targetPos));
		}

		private IEnumerator JumpRoutine(Vector2 startPos, Vector2 targetPos)
		{
			float elapsed = 0f;
			const float JUMP_DURATION = 1.2f;
			const float JUMP_HEIGHT = 5f;

			while (elapsed < JUMP_DURATION)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / JUMP_DURATION;
				Vector2 currentPos = Vector2.Lerp(startPos, targetPos, t);
				float height = 4f * JUMP_HEIGHT * t * (1 - t);
				currentPos.y += height;
				transform.position = currentPos;
				yield return null;
			}

			transform.position = targetPos;
			targetLocation = pitchforkLocation;
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
			const double TOLERANCE = 2.5;
			double toleranceSqr = TOLERANCE * TOLERANCE;

			return ((Vector2)transform.position - position).sqrMagnitude <= toleranceSqr;
		}

		public override void DoDialog()
		{
			DialogManager.Instance.PopUpDialog("EW - WHAT IS THAT??", gameObject.transform.position);
			GetComponent<Animator>().SetTrigger(FarmerBossTrigger.Annoyed);
			gameObject.tag = GameTag.Boss;
		}
	}
}