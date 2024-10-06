using UnityEngine;

public class Farmer : PersonaAbstract
{
    private const float MEELE_ATTACK_RANGE = 2f;
    private bool isAngry = false;
    public ParticleSystem angryParticleEffect;
    public override string PersonaName { get; set; } = "Farmer";

    public override void BaseAttack()
    {
        MeeleAttack();
    }

    public override void FirstAttack()
    {
        if (!isAngry)
        {
            AngerHim();
        }
        else
        {
            CalmHim();
        }
    }

    public override void SecondAttack()
    {
        Debug.Log("Unfinished");
        return;
    }
    public override void SwapToMe()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void SwapFromMe()
    {
        CalmHim();
    }

    private void CalmHim()
    {
        isAngry = false;
        angryParticleEffect.Stop();
    }

    private void AngerHim()
    {
        isAngry = true;
        angryParticleEffect.Play();
    }

    private void MeeleAttack()
    {
        const float KNOCKBACK = 2;
        DealDamageTo(DetectEnemiesInRange(MEELE_ATTACK_RANGE), KNOCKBACK);
    }

    void OnDrawGizmosSelected()
    {
        if (playerBase.attackPoint.position == null) { return; }
        Gizmos.DrawWireSphere(playerBase.attackPoint.position, MEELE_ATTACK_RANGE);
    }
}