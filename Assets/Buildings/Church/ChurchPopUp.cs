using System.IO;
using Buildings.Church;
using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChurchPopUp : MonoBehaviour
{
	public Button leftChoice;
	public Button rightChoice;
	public Button cancelButton;

	private Church church;
	private const string FOLDER_PATH_FROM_RESOURCE = "ChurchGodImages";

	public void OpenPopUp(Church churchRef, ChurchGod firstGod, ChurchGod secondGod)
	{
		gameObject.SetActive(true);
		church = churchRef;
		LoadGods(leftChoice, firstGod);
		LoadGods(rightChoice, secondGod);
	}

	private void LoadGods(Button button, ChurchGod churchGod)
	{
		string path = Path.Combine(FOLDER_PATH_FROM_RESOURCE, churchGod.name);
		button.image.sprite = Resources.Load<Sprite>(path);
		button.GetComponentInChildren<TextMeshProUGUI>().text = churchGod.name;
	}

	public void ClosePopUp()
	{
		church?.OnPopUpClosed();
		gameObject.SetActive(false);
	}
}