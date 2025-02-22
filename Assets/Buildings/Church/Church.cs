using UnityEngine;

public class Church : BuildingAbstract
{
    [SerializeField] private ChurchPopUp popUp;

    public override void Use(PlayerBase pBase)
    {
        base.Use(pBase);
        playerBase.canMove = false;
        popUp.OpenPopUp(this);
    }

    public void OnPopUpClosed()
    {
        playerBase.canMove = true;
    }
}