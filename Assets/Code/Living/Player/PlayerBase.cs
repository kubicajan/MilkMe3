using DefaultNamespace;
using Living;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Living.Player
{
	public class PlayerBase : LivingEntity
	{
		[SerializeField] public Transform attackPoint;
		[SerializeField] public ParticleSystem jumpParticle;
		[SerializeField] public GameObject healParticlePrefab;
		[SerializeField] public Transform groundCheck;
		[SerializeField] public LayerMask buildingLayers;
		[SerializeField] public LayerMask npcLayers;
		public bool canMove = true;
		private ItemData[] inventory = new ItemData[3];

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

		public bool TryAddToInventory(ItemData item)
		{
			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					inventory[i] = item;
					return true;
				}
			}

			return false;
		}
	}
}