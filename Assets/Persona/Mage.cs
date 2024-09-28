
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mage : PersonaAbstract
{
    public override string PersonaName { get; set; } = "Mage";

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