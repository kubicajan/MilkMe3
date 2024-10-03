
using UnityEngine;

public class Farmer : PersonaAbstract
{
    private const float MEELE_ATTACK_RANGE = 2f;

    public override string PersonaName { get; set; } = "Farmer";

    public override void BaseAttack()
    {
        MeeleAttack();
    }

    public override void FirstAttack()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void SecondAttack()
    {
        return;
        throw new System.NotImplementedException();
    }


    private void MeeleAttack()
    {
        DealDamageTo(Utility.DetectByLayers(playerBase.attackPoint.position, MEELE_ATTACK_RANGE, playerBase.enemyLayers));
    }

    void OnDrawGizmosSelected()
    {
        if (playerBase.attackPoint.position == null) { return; }
        Gizmos.DrawWireSphere(playerBase.attackPoint.position, MEELE_ATTACK_RANGE);
    }
}