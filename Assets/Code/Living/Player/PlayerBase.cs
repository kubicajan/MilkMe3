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

		protected override void Awake()
		{
			base.Awake();
			Init(_health: 100,
				_boxCollider: GetComponent<BoxCollider2D>());
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