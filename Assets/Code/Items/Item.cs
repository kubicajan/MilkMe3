using Code.Living.Player;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

public class Item : MonoBehaviour
{
	public ItemData data;

	public void Initialize()
	{
		//TODO: read from json from possibleRarities.
		data = new ItemData();
		Random random = new Random();
		data.name = random.Next('A', 'Z' + 1).ToString();
	}

	public void Interact(PlayerBase playerBase)
	{
		if (playerBase.TryAddToInventory(data))
		{
			Debug.Log($"item {data.name} picked up");
			Destroy(transform.parent.gameObject);
		}
		else
		{
			Debug.Log($"cannot pick up {data.name} ");
		}
		//TODO: display message that inventory is full
	}
}