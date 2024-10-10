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

    public void CreateTextMessage(string textToDisplay, GameObject parent)
    {
        GameObject duplicatedObject = Instantiate(dialogComponent);
        //todo: tady se to jebe trochu
        //duplicatedObject.transform.SetParent(parent.transform, false);
        Vector2 parentPosition = parent.transform.position;
        duplicatedObject.transform.position = new Vector2(parentPosition.x, parentPosition.y + 2);
        var gg = duplicatedObject.GetComponentInChildren<TextMeshProUGUI>();
        gg.text = textToDisplay;

    }
}