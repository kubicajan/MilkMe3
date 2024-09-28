using UnityEngine;

public abstract class PersonaAbstract : MonoBehaviour, PersonaInterface
{
    public abstract string PersonaName { get; set; }

    [SerializeField]
    private Sprite skin;

    public Sprite GetSkin()
    {
        return skin;
    }

    public abstract void BaseAttack();

    public abstract void FirstAttack();

    public abstract void SecondAttack();
}