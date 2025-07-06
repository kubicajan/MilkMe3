using UnityEngine;

public class Item : MonoBehaviour
{
	void Start()
	{
	}

	void Update()
	{
	}

	public void Interact()
	{
		Destroy(gameObject);
	}
}