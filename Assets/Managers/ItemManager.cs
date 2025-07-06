using System.Collections;
using Helpers;
using Living.Enemies;
using Persona.Blueprints;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance { get; private set; }
	public static bool IsInitialized => Instance != null;

	[SerializeField] private GameObject item;

	private void Awake()
	{
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
		Instantiate(item, parentPosition, Quaternion.identity);

		int pairs = (numberOfItems - 1) / 2;
		float offset = 0;

		for (int i = 0; i < pairs; i++)
		{
			yield return new WaitForSeconds(0.5f);
			offset = 1.5f * (i + 1);
			InstantiateItem(parentPosition, -offset);
			InstantiateItem(parentPosition, offset);
		}

		if (numberOfItems % 2 == 0)
		{
			yield return new WaitForSeconds(0.5f);
			offset += 1.5f;
			InstantiateItem(parentPosition, -offset);
		}
	}

	private void InstantiateItem(Vector2 position, float offset)
	{
		Instantiate(item, new Vector2(position.x + offset, position.y), Quaternion.identity);
	}
}