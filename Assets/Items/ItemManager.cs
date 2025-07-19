using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using Living.Enemies;
using Persona.Blueprints;
using UnityEngine;
using UnityEngine.Serialization;


public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance { get; private set; }
	public static bool IsInitialized => Instance != null;

	[SerializeField] private GameObject itemPrefab;

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
		GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
		newItem.GetComponentInChildren<Item>().Initialize();
		newItem.SetActive(true);
	}
}