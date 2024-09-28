
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Farmer : PersonaAbstract
{
    public override string PersonaName { get; set; } = "Farmer";

    public override void BaseAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void FirstAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void SecondAttack()
    {
        throw new System.NotImplementedException();
    }
}