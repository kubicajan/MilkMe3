using UnityEngine;

public abstract class BuildingAbstract : MonoBehaviour
{
    protected PlayerBase playerBase;

    public virtual void Build()
    {
        Debug.Log("I AM BUILDING");
    }

    public virtual void Use(PlayerBase pBase)
    {
        if (!playerBase)
        {
            playerBase = pBase;
        }
    }
}