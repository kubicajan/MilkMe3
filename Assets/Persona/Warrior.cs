
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Warrior : PersonaAbstract
{
    private const float MEELE_ATTACK_RANGE = 2f;
    public ParticleSystem stompParticle;
    public override string PersonaName { get; set; } = "Warrior";

    public override void BaseAttack()
    {
        MeeleAttack();
    }

    public override void FirstAttack()
    {
        StompAttack();
    }

    public override void SecondAttack()
    {
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

    private void StompAttack()
    {
        const float AREA_OF_EFFECT = 5f;
        if (!IsGrounded())
        {
            StartCoroutine(StompDown(AREA_OF_EFFECT));
            return;
        }

        Instantiate(stompParticle, transform.position, Quaternion.identity);
        DealDamageTo(Utility.DetectByLayers(transform.position, AREA_OF_EFFECT, playerBase.enemyLayers));
    }

    private IEnumerator StompDown(float areaOfEffect)
    {
        Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);

        RigidBody.velocity = new Vector2(0, -100);

        while (!IsGrounded())
        {
            yield return null;
        }
        RigidBody.velocity = new Vector2();
        Instantiate(stompParticle, transform.position, Quaternion.identity);
        DealDamageTo(Utility.DetectByLayers(transform.position, areaOfEffect, playerBase.enemyLayers));
        Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
    }
}