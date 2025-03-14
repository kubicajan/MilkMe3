using System;
using System.Collections;
using UnityEngine;

public class EnemyScript : LivingEntity
{
	public Transform groundCheck;
	public Transform playerLocation;
	public LayerMask groundLayers;
	protected float speed = 1f;
	public ParticleSystem explosionParticles;
	protected static float lastDirection = 1;
	private Vector2 movement;


	private void Start()
	{
		Init(_health: 500,
			_rigidBody2D: GetComponent<Rigidbody2D>(),
			_boxCollider: GetComponent<BoxCollider2D>());
	}

	public void Update()
	{
		if (!Mathf.Approximately(lastDirection, movement.x))
		{
			lastDirection = movement.x;
			transform.Rotate(0f, 180f, 0f);
		}


		if (Math.Abs(playerLocation.position.x) - Math.Abs(transform.position.x) > 0)
		{
			movement.x = -1;
		}
		else
		{
			movement.x = 1;
		}
	}

	public void Move()
	{
		Debug.Log("helo");
		if (!IsImmobilized())
		{
			Vector3 targetPosition = new Vector3(playerLocation.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		}
		else
		{
			if (IsGrounded() && RigidBody.velocity == Vector2.zero)
			{
				Immobilize(false);
			}
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Instantiate(explosionParticles, transform.position, Quaternion.identity);
			MagicPushMe(collision.gameObject.transform.position, 5);
		}
	}

	public void LiftMeUp(int liftByThisMuch)
	{
		RunMovementCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform, this));
	}

	public void AttackMoveMe(float moveBy, float directionToMove)
	{
		RunMovementCoroutine(Common.WarriorMoveAttack(this.transform.position.x, moveBy, directionToMove, transform,
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
			RigidBody.velocity = new Vector2(0, stompSpeed);
			Debug.Log("still stomping");
			yield return null;
		}

		RigidBody.velocity = Vector2.zero;
	}

	protected void RunMovementCoroutine(IEnumerator coroutine)
	{
		StopMovementCoroutine();
		movementCoroutine = StartCoroutine(coroutine);
	}

	protected void StopMovementCoroutine()
	{
		if (movementCoroutine != null)
		{
			StopCoroutine(movementCoroutine);
		}
	}

	protected bool IsGrounded()
	{
		return Utility.IsGroundedOnLayers(groundCheck.position, groundLayers);
	}
}