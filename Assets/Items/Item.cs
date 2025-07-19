using System;
using System.Collections.Generic;
using DefaultNamespace;
using Living.Player;
using UnityEngine;
using Random = System.Random;

public class Item : MonoBehaviour
{
	public ItemData data;

	//TODO: enum to switch between boss items or anything specific.
	// there should a json that loads random itemData from section 1 (all normal items), section 2 (boss items) ...
	// this should be handled in the generation in Awake
	//Enum XXX switch

	public void Initialize(List<ItemRarity> possibleRarities)
	{
		//TODO: read from json from possibleRarities.
		//actually this should be handled inside of Manager as it should have all the fields inside. the Data should just be passed here to initialize.
		//or maybe within AWAKE?
		data = new ItemData();
		Random random = new Random();
		data.name = random.Next('A', 'Z' + 1).ToString();
	}


	void Start()
	{
	}

	void Update()
	{
	}

	public void Interact(PlayerBase playerBase)
	{
		if (playerBase.TryAddToInventory(data))
		{
			Debug.Log(data.name);
			Destroy(gameObject);
		}
		//TODO: display message that inventory is full
	}
}