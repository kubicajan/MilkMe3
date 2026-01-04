using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Items
{
	public class ItemManager : MonoBehaviour
	{
		public static ItemManager Instance { get; private set; }
		public static bool IsInitialized => Instance != null;

		[SerializeField] private GameObject itemPrefab;

		[SerializeField] private List<ParticleEffectByRarity> itemRarityEffects;

		private void Awake()
		{
			itemPrefab.SetActive(false);
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public IEnumerator SpawnItems(int numberOfItems, Vector2 parentPosition)
		{
			yield return new WaitForSeconds(0.8f);
			InstantiateItem(parentPosition);

			int pairs = (numberOfItems - 1) / 2;
			float offset = 0;

			for (int i = 0; i < pairs; i++)
			{
				yield return new WaitForSeconds(0.5f);
				offset = 1.5f * (i + 1);
				InstantiateItemWithOffset(parentPosition, -offset);
				InstantiateItemWithOffset(parentPosition, offset);
			}

			if (numberOfItems % 2 == 0)
			{
				yield return new WaitForSeconds(0.5f);
				offset += 1.5f;
				InstantiateItemWithOffset(parentPosition, -offset);
			}
		}

		private void InstantiateItemWithOffset(Vector2 position, float offset)
		{
			InstantiateItem(new Vector2(position.x + offset, position.y));
		}

		private void InstantiateItem(Vector2 position)
		{
			GameObject newItemWrapper = Instantiate(itemPrefab, position, Quaternion.identity);
			Item item = newItemWrapper.GetComponentInChildren<Item>();
			item.Initialize();

			ParticleEffectByRarity effectPrefab =
				itemRarityEffects[Random.Range(0, itemRarityEffects.Count)];

			ParticleSystem effectInstance = Instantiate(effectPrefab.particleEffect, newItemWrapper.transform);
			effectInstance.transform.SetParent(item.transform);
			effectInstance.transform.localScale = Vector3.one;

			newItemWrapper.SetActive(true);
		}

		[System.Serializable]
		public class ParticleEffectByRarity
		{
			public ItemRarity rarity;
			public ParticleSystem particleEffect;
		}
	}
}