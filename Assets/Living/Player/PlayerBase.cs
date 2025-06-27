using UnityEngine;

namespace Living.Player
{
	public class PlayerBase : LivingEntity
	{
		[SerializeField] public Transform attackPoint;
		[SerializeField] public ParticleSystem jumpParticle;
		[SerializeField] public Transform groundCheck;
		[SerializeField] public LayerMask buildingLayers;
		[SerializeField] public LayerMask npcLayers;
		public bool canMove = true;

		private void Awake()
		{
			Init(_health: 100,
				_rigidBody2D: GetComponent<Rigidbody2D>(),
				_boxCollider: GetComponent<BoxCollider2D>());
		}

		public Rigidbody2D GetRigidBody()
		{
			return RigidBody;
		}
	}
}