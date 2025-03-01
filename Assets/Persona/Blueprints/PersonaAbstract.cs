using System;
using System.Collections;
using System.Linq;
using Living.Player;
using UnityEngine;

namespace Persona.Blueprints
{
	public abstract class PersonaAbstract : MonoBehaviour, IPersonaInterface
	{
		protected PlayerBase playerBase;
		public abstract string PersonaName { get; set; }
		protected virtual int maxNumberOfJumps => 5;

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

		public void Initialize(PlayerBase _playerBase)
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
			Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);
			Common.TurnOffGravity(RigidBody, true);
			//dashing = true;
			RigidBody.velocity = new Vector2(lastDirection * dashForce, 0);
			yield return new WaitForSeconds(0.1f);
			RigidBody.velocity = Vector2.zero;
			yield return new WaitForSeconds(0.1f);
			//dashing = false;
			Common.TurnOffGravity(RigidBody, false);
			Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
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
				transform.Rotate(0f, 180f, 0f);
			}

			Vector3 m_Velocity = Vector3.zero;
			Vector3 targetVelocity = new Vector2(movement.x * moveSpeed, RigidBody.velocity.y);
			RigidBody.velocity = Vector3.SmoothDamp(RigidBody.velocity, targetVelocity, ref m_Velocity, .05f);
		}

		public void MovePotentially()
		{
			movement.x = Input.GetAxisRaw("Horizontal"); // A (-1) and D (+1)
		}

		public void Heal()
		{
			playerBase.Heal(10);
		}

		public void Build()
		{
			Collider2D closestBuilding = DetectClosest(playerBase.buildingLayers);
			closestBuilding?.GetComponent<BuildingAbstract>().Build();
		}

		public void Interact()
		{
			Collider2D closestNpc = DetectClosest(playerBase.npcLayers);

			if (closestNpc)
			{
				closestNpc.GetComponent<NpcScript>().DoDialog();
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
			else if (!IsGrounded() && maxNumberOfJumps <= consecutiveJumps)
			{
				return;
			}

			consecutiveJumps++;
			RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);
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
			return Utility.DetectByLayers(playerBase.attackPoint.position, range, playerBase.enemyLayers);
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

		public abstract void BaseAttack();

		public abstract void FirstAbility();

		public abstract void SecondAbility();

		public abstract void SwapToMe();

		public abstract void SwapFromMe();
	}
}