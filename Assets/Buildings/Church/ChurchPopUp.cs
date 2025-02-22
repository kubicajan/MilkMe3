using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ChurchPopUp : MonoBehaviour
{
    public Button leftChoice;
    public Button rightChoice;
    public Button cancelButton;

    private Church church;

    public void OpenPopUp(Church churchRef)
    {
        gameObject.SetActive(true);
        church = churchRef;
    }

    public void ClosePopUp()
    {
        church?.OnPopUpClosed();
        gameObject.SetActive(false);
    }
}