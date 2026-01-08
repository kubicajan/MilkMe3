using System;
using System.Collections;
using Helpers;
using Helpers.CommonEnums;
using Living;
using UnityEngine;

namespace Code.Living.Enemies
{
	public class EnemyScript : LivingEntity
	{
		[SerializeField] protected Transform groundCheck;
		[SerializeField] protected Transform playerLocation;

		protected float movementSpeed = 1f;
		public ParticleSystem explosionParticles;
		protected float lastDirection = 1;
		private int movementDirection;
		protected Transform targetLocation;

		private void Start()
		{
			SetPlayerAsTarget();

			Init(_health: 50,
				_boxCollider: GetComponent<BoxCollider2D>());
		}

		public void SetPlayerAsTarget()
		{
			targetLocation = playerLocation;
			TurnTowardsTarget();
		}

		//tu by se mozna mel dat watcher co checkuje jestli se movementDirection zmenil a jestli jo, tak jedu
		protected void TurnTowardsTarget()
		{
			if (targetLocation.position.x > transform.position.x)
			{
				movementDirection = 1;
			}
			else
			{
				movementDirection = -1;
			}

			if (!Mathf.Approximately(lastDirection, movementDirection))
			{
				lastDirection = movementDirection;
				transform.Rotate(0f, 180f, 0f);
			}
		}

		public virtual void DoDialog()
		{
			return;
		}

		private Vector2 GetMoveDestination()
		{
			return new Vector2(targetLocation.position.x, transform.position.y);
		}

		private Vector3 velocitySmooth;

		public void Move()
		{
			Rigidbody2D rigidBody = GetRigidBody();
			if (!IsImmobilized())
			{
				TurnTowardsTarget();

				Vector3 moveDir = (GetMoveDestination() - rigidBody.position).normalized;
				float distance = Vector3.Distance(rigidBody.position, GetMoveDestination());

				// Compute target velocity based on distance
				Vector3 targetVelocity = moveDir * movementSpeed;

				// Smooth the velocity
				rigidBody.linearVelocity =
					Vector3.SmoothDamp(rigidBody.linearVelocity, targetVelocity, ref velocitySmooth, 0.05f);

				// Optional: stop when close enough
				if (distance < 0.1f)
				{
					rigidBody.linearVelocity = Vector3.zero;
				}
			}
			else
			{
				if (IsGrounded() && rigidBody.linearVelocity == Vector2.zero)
				{
					Immobilize(false);
				}
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (CompareTag(GameTag.Enemy) || CompareTag(GameTag.Boss))
			{
				if (collision.gameObject.CompareTag(GameTag.Player))
				{
					if (collision.gameObject.TryGetComponent<LivingEntity>(out var targetScript))
					{
						Instantiate(explosionParticles, transform.position, Quaternion.identity);
						targetScript.TakeDamage(10);
						targetScript.GetKnockedBack(gameObject.transform.position, 5);
					}
				}
				else if (collision.gameObject.CompareTag(GameTag.PlayerProjectile))
				{
					if (collision.gameObject.TryGetComponent<TornadoScript>(out var tornadoScript))
					{
						this.TakeDamage(10);
						LiftMeUp(50);
						Instantiate(explosionParticles, transform.position, Quaternion.identity);
						// this.GetKnockedBack(gameObject.transform.position, 15);
					}
				}
			}
		}

		public void LiftMeUp(int liftByThisMuch)
		{
			RunMovementCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, GetRigidBody(), transform, this));
		}

		public void AttackMoveMe(float moveBy, float directionToMove)
		{
			RunMovementCoroutine(Common.WarriorMoveAttack(transform.position.x, moveBy, directionToMove, transform,
				GetRigidBody(), this));
		}

		public void StompMeDown(int stompSpeed)
		{
			StartCoroutine(StompDown(stompSpeed));
		}

		private IEnumerator StompDown(int stompSpeed)
		{
			while (!IsGrounded())
			{
				GetRigidBody().linearVelocity = new Vector2(0, stompSpeed);
				yield return null;
			}

			GetRigidBody().linearVelocity = Vector2.zero;
		}

		private void RunMovementCoroutine(IEnumerator coroutine)
		{
			StopMovementCoroutine();
			movementCoroutine = StartCoroutine(coroutine);
		}

		private void StopMovementCoroutine()
		{
			if (movementCoroutine != null)
			{
				StopCoroutine(movementCoroutine);
			}
		}

		private bool IsGrounded()
		{
			return Utility.IsGroundedOnLayers(groundCheck.position, groundLayers);
		}
	}
}