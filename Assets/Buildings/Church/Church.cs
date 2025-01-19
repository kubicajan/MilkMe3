using UnityEngine;

public class Church : BuildingAbstract
{
    [SerializeField] private ChurchPopUp popUp;

    public override void Use()
    {
        popUp.OpenPopUp();
    }
}