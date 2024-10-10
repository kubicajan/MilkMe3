using System.Xml;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }
    public static bool IsInitialized => Instance != null;

    [SerializeField]
    private GameObject dialogComponent;

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

    public void CreateTextMessage(string textToDisplay, Vector2 parentPosition)
    {
        GameObject duplicatedObject = Instantiate(dialogComponent);
        //duplicatedObject.transform.SetParent(parent.transform, false);
        duplicatedObject.transform.position = new Vector2(parentPosition.x, parentPosition.y);
        duplicatedObject.GetComponentInChildren<TextMeshProUGUI>().text = textToDisplay;
    }
}