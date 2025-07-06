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

	public IEnumerator PopUpItem(int numberOfItems, Vector2 parentPosition)
	{
		yield return new WaitForSeconds(0.5f);

		for (int i = 0; i < numberOfItems; i++)
		{
			yield return new WaitForSeconds(0.5f);
			GameObject duplicatedObject = Instantiate(item);
			duplicatedObject.transform.position = Utility.RandomizeXPosition(parentPosition);
		}
	}
}