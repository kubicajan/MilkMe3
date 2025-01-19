using UnityEngine;

public abstract class BuildingAbstract : MonoBehaviour
{
    public virtual void Build()
    {
        Debug.Log("I AM BUILDING");
    }

    public virtual void Use()
    {
        return;
    }
}