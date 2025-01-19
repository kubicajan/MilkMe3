using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class Church : BuildingAbstract
{
    [SerializeField] private ChurchPopUp popUp;

    public override void Use()
    {
        popUp.gameObject.SetActive(true);
    }
}