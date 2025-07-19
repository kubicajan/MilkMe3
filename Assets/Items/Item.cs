using DefaultNamespace;
using Living.Player;
using UnityEngine;
using Random = System.Random;

public class Item : MonoBehaviour
{
	public ItemData data;

	//TODO: RARITY should be handled by chance, each tier should have +-15% difference in stats?
	// a filename should be passed here to spawn from a specific itempool (FIRSTBOSS, ...)
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
			Destroy(gameObject);
		}
		//TODO: display message that inventory is full
	}
}