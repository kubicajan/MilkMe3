
using UnityEngine;

public class Mage : PersonaAbstract
{
    public override string PersonaName { get; set; } = "Mage";
    public ParticleSystem shieldParticleEffect;

    public override void BaseAttack()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void FirstAttack()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void SecondAttack()
    {
        MagicPushBack();
    }

    public override void SwapToMe()
    {
        ActivateShield();
    }

    public override void SwapFromMe()
    {
        DeactivateShield();
    }

    private void ActivateShield()
    {
        shieldParticleEffect.Play();
        shieldParticleEffect.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void DeactivateShield()
    {
        shieldParticleEffect.Stop();
        shieldParticleEffect.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void MagicPushBack()
    {
        const float DISTANCE_LIMIT = 10f;
        MagicPushAllEnemies(DetectEnemiesInRange(DISTANCE_LIMIT / 2), transform.position, DISTANCE_LIMIT);
    }

    private void MagicPushAllEnemies(Collider2D[] detectedEntities, Vector2 perpetratorPosition, float distanceLimit)
    {
        ProcessEnemies(detectedEntities, enemyScript => enemyScript.MagicPushMe(perpetratorPosition, distanceLimit));
    }
}