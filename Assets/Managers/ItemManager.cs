using System.Xml;
using TMPro;
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

	public void PopUpItem(Vector2 parentPosition)
	{
		GameObject duplicatedObject = Instantiate(item);
		//duplicatedObject.transform.SetParent(parent.transform, false);
		duplicatedObject.transform.position = new Vector2(parentPosition.x, parentPosition.y);
	}
}