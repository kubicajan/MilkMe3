
using System.Collections;
using System.Linq;
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
        LiftAttack();
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

    private void LiftAttack()
    {
        Collider2D[] detectedEnemies = Utility.DetectByLayers(playerBase.attackPoint.position, MEELE_ATTACK_RANGE, playerBase.enemyLayers);
        const int LIFT_BY_THIS_MUCH = 5;

        foreach (Collider2D enemyCollider in detectedEnemies)
        {
            if (enemyCollider.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                enemyScript.LiftMeUp(LIFT_BY_THIS_MUCH);
            }
        }
        LiftMeUp(LIFT_BY_THIS_MUCH);
    }

    public void LiftMeUp(int liftByThisMuch)
    {
        StartCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform));
    }

    private void StompAttack()
    {
        const float AREA_OF_EFFECT = 5f;
        const int STOMP_SPEED = -100;

        if (!IsGrounded())
        {
            Collider2D[] detectedEnemies = Utility.DetectByLayers(playerBase.attackPoint.position, MEELE_ATTACK_RANGE, playerBase.enemyLayers);

            foreach (Collider2D enemyCollider in detectedEnemies)
            {
                if (enemyCollider.TryGetComponent<EnemyScript>(out var enemyScript))
                {
                    enemyScript.StompMeDown(STOMP_SPEED);
                }
            }
            StartCoroutine(StompDown(AREA_OF_EFFECT, STOMP_SPEED));

            return;
        }

        Instantiate(stompParticle, transform.position, Quaternion.identity);
        DealDamageTo(Utility.DetectByLayers(transform.position, AREA_OF_EFFECT, playerBase.enemyLayers));
    }

    private IEnumerator StompDown(float areaOfEffect, int stompSpeed)
    {
        Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);

        while (!IsGrounded())
        {
            RigidBody.velocity = new Vector2(0, stompSpeed);
            yield return null;
        }
        RigidBody.velocity = new Vector2();
        yield return new WaitForSeconds(0.05f);
        Instantiate(stompParticle, transform.position, Quaternion.identity);
        DealDamageTo(Utility.DetectByLayers(transform.position, areaOfEffect, playerBase.enemyLayers));
        Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
    }
}