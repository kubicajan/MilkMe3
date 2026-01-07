using System;
using System.Collections;
using Code;
using Helpers;
using Helpers.CommonEnums;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Living.Enemies
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
				_rigidBody2D: GetComponent<Rigidbody2D>(),
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

		public void Move()
		{
			if (!IsImmobilized())
			{
				TurnTowardsTarget();
				transform.position =
					Vector3.MoveTowards(transform.position, GetMoveDestination(), movementSpeed * Time.deltaTime);
			}
			else
			{
				if (IsGrounded() && RigidBody.linearVelocity == Vector2.zero)
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
			RunMovementCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform, this));
		}

		public void AttackMoveMe(float moveBy, float directionToMove)
		{
			RunMovementCoroutine(Common.WarriorMoveAttack(transform.position.x, moveBy, directionToMove, transform,
				RigidBody, this));
		}

		public void StompMeDown(int stompSpeed)
		{
			StartCoroutine(StompDown(stompSpeed));
		}

		private IEnumerator StompDown(int stompSpeed)
		{
			while (!IsGrounded())
			{
				RigidBody.linearVelocity = new Vector2(0, stompSpeed);
				yield return null;
			}

			RigidBody.linearVelocity = Vector2.zero;
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