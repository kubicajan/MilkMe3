using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ChurchPopUp : MonoBehaviour
{
    public Button leftChoice;
    public Button rightChoice;
    public Button cancelButton;

    public void OpenPopUp()
    {
       gameObject.SetActive(true);
    }

    public void ClosePopUp()
    {
        gameObject.SetActive(false);
    }
}