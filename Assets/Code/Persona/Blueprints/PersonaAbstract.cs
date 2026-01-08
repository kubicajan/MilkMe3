using System;
using System.Collections;
using System.Linq;
using Code.Living.Enemies;
using Code.Living.NPCs;
using Code.Living.Player;
using Helpers;
using Living.Enemies;
using Persona.Blueprints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Persona.Blueprints
{
	public abstract class PersonaAbstract : MonoBehaviour, IPersonaInterface
	{
		public abstract string PersonaName { get; set; }
		protected virtual int MaxNumberOfJumps => 5;
		protected PlayerBase playerBase;

		private float moveSpeed = 10f;
		private float jumpForce = 20f;
		private float dashForce = 100f;
		private Vector2 movement;
		//public bool dashing = false;

		//TODO: tohle nesmi byt static, jinak nebude multiplier fungovat vubec.
		protected static float lastDirection = 1;
		private static int consecutiveJumps = 1;
		protected Rigidbody2D RigidBody;

		[SerializeField] private Sprite skin;

		public virtual void Initialize(PlayerBase _playerBase)
		{
			playerBase = _playerBase;
			RigidBody = playerBase.GetRigidBody();
		}

		protected void RunMovementCoroutine(IEnumerator coroutine)
		{
			StopMovementCoroutine();
			playerBase.movementCoroutine = StartCoroutine(coroutine);
		}

		private void StopMovementCoroutine()
		{
			if (playerBase.movementCoroutine != null)
			{
				StopCoroutine(playerBase.movementCoroutine);
			}
		}

		protected bool IsGrounded()
		{
			return Utility.IsGroundedOnLayers(playerBase.groundCheck.position, playerBase.groundLayers);
		}

		public void Dash()
		{
			RunMovementCoroutine(DashCoroutine());
		}

		private IEnumerator DashCoroutine()
		{
			ResetJumps();
			Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.hostileLayers);
			Common.TurnOffGravity(RigidBody, true);
			//dashing = true;
			RigidBody.linearVelocity = new Vector2(lastDirection * dashForce, 0);
			yield return new WaitForSeconds(0.1f);
			RigidBody.linearVelocity = Vector2.zero;
			yield return new WaitForSeconds(0.1f);
			//dashing = false;
			Common.TurnOffGravity(RigidBody, false);
			Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.hostileLayers);
		}

		private void ResetJumps()
		{
			consecutiveJumps = 0;
		}

		public Vector2 GetMovement()
		{
			return movement;
		}

		public void Move()
		{
			if (!Mathf.Approximately(lastDirection, movement.x))
			{
				lastDirection = movement.x;

				Vector3 scale = transform.localScale;
				scale.x = Mathf.Sign(movement.x) * Mathf.Abs(scale.x); // flip based on movement direction
				transform.localScale = scale;
			}

			Vector3 m_Velocity = Vector3.zero;
			Vector3 targetVelocity = new Vector2(movement.x * moveSpeed, RigidBody.linearVelocity.y);
			RigidBody.linearVelocity =
				Vector3.SmoothDamp(RigidBody.linearVelocity, targetVelocity, ref m_Velocity, .05f);
		}

		public void MovePotentially()
		{
			float horizontal = 0f;

			if (Keyboard.current.aKey.isPressed)
				horizontal -= 1f; // A = left
			if (Keyboard.current.dKey.isPressed)
				horizontal += 1f; // D = right

			movement.x = horizontal;
		}

		public void Heal()
		{
			playerBase.Heal(10);
			Instantiate(playerBase.healParticlePrefab,
				playerBase.groundCheck.transform.position, Quaternion.identity);
		}

		public void Build()
		{
			Collider2D closestBuilding = DetectClosest(playerBase.buildingLayers);
			closestBuilding?.GetComponent<BuildingAbstract>().Build();
		}

		public void Interact()
		{
			Collider2D closestItem = DetectClosest(playerBase.itemLayers);

			if (closestItem)
			{
				closestItem.GetComponent<Item>().Interact(playerBase);
				return;
			}

			Collider2D closestNpc = DetectClosest(playerBase.npcLayers);

			if (closestNpc)
			{
				closestNpc.GetComponent<NpcScript>().DoDialog();
				return;
			}

			Collider2D closestEnemy = DetectClosest(playerBase.hostileLayers);

			if (closestEnemy)
			{
				closestEnemy.GetComponent<EnemyScript>().DoDialog();
				return;
			}

			Collider2D closestBuilding = DetectClosest(playerBase.buildingLayers);
			closestBuilding?.GetComponent<BuildingAbstract>().Use(playerBase);
		}

		public void CommitSuicide()
		{
			playerBase.TakeDamage(playerBase.GetCurrentHealth());
		}

		public void Jump()
		{
			if (IsGrounded())
			{
				ResetJumps();
			}
			else if (!IsGrounded() && MaxNumberOfJumps <= consecutiveJumps)
			{
				return;
			}

			if (consecutiveJumps > 0)
			{
				Instantiate(playerBase.jumpParticle, transform.position, Quaternion.identity);
			}

			consecutiveJumps++;
			RigidBody.linearVelocity = new Vector2(RigidBody.linearVelocity.x, jumpForce);
		}

		public Sprite GetSkin()
		{
			return skin;
		}

		protected void DealDamageTo(Collider2D[] detectedEnemies, float knockBack)
		{
			foreach (Collider2D enemy in detectedEnemies)
			{
				if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
				{
					enemyScript.TakeDamage(10);
					enemyScript.GetKnockedBack(this.transform.position, knockBack);
					Debug.Log("hit enemy");
				}
			}
		}

		protected Collider2D[] DetectEnemiesInRange(float range)
		{
			return Utility.DetectByLayers(playerBase.attackPoint.position, range, playerBase.hostileLayers);
		}

		private Collider2D DetectClosest(LayerMask layers)
		{
			return Utility.DetectByLayers(transform.position, 1, layers).FirstOrDefault();
		}

		protected void ProcessEnemies(Collider2D[] detectedEntities, Action<EnemyScript> action)
		{
			foreach (Collider2D enemyCollider in detectedEntities)
			{
				if (enemyCollider.TryGetComponent<EnemyScript>(out var enemyScript))
				{
					action?.Invoke(enemyScript);
				}
			}
		}
		//
		// //WARNING: OBSOLETE
		// protected Quaternion GetRotation()
		// {
		// 	float angle = (1 - lastDirection) * 90;
		// 	return Quaternion.Euler(0, 0, angle);
		// }

		protected void SetFacingDirection(Transform objToFlip)
		{
			Vector3 localScale = transform.localScale;
			localScale.x = Mathf.Abs(localScale.x) * (Mathf.Approximately(lastDirection, 1) ? 1 : -1);
			objToFlip.localScale = localScale;
		}

		public abstract void BaseAttack();

		public abstract void FirstAbility();

		public abstract void SecondAbility();

		public abstract void SwapToMe();

		public abstract void SwapFromMe();
	}
}