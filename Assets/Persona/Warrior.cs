
using System;
using System.Collections;
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

    public override void SwapToMe()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void SwapFromMe()
    {
        Debug.Log("Unfinished");
        return;
    }

    private void MeeleAttack()
    {
        const float KNOCKBACK = 0.2f;
        const int MOVE_BY = 2;
        Collider2D[] detectedEnemies = DetectEnemiesInRange(MEELE_ATTACK_RANGE);
        DealDamageTo(detectedEnemies, KNOCKBACK);
        AttackMoveAllEnemiesHit(detectedEnemies, MOVE_BY);
        MoveAttackMeBy(MOVE_BY);
    }

    private void MoveAttackMeBy(int moveBy)
    {
        StartCoroutine(Common.MoveAttack(transform.position.x, moveBy, lastDirection, transform, RigidBody));
    }

    private void LiftAttack()
    {
        const int LIFT_BY_THIS_MUCH = 5;
        Collider2D[] detectedEnemies = DetectEnemiesInRange(MEELE_ATTACK_RANGE);
        LiftUpAllEnemiesHit(detectedEnemies, LIFT_BY_THIS_MUCH);
        LiftMeUpBy(LIFT_BY_THIS_MUCH);
    }

    private void LiftMeUpBy(int liftByThisMuch)
    {
        StartCoroutine(Common.LiftUp(liftByThisMuch, transform.position.y, RigidBody, transform));
    }

    private void StompAttack()
    {
        const float AREA_OF_EFFECT = 5f;
        const float KNOCKBACK = 2;
        const int STOMP_SPEED = -100;

        if (!IsGrounded())
        {
            StompDownAllEnemiesHit(DetectEnemiesInRange(MEELE_ATTACK_RANGE), STOMP_SPEED);
            StartCoroutine(StompDown(AREA_OF_EFFECT, STOMP_SPEED, KNOCKBACK));
            return;
        }
        else
        {
            Instantiate(stompParticle, transform.position, Quaternion.identity);
            DealDamageTo(DetectEnemiesInRange(AREA_OF_EFFECT), KNOCKBACK);
        }
    }

    private IEnumerator StompDown(float areaOfEffect, int stompSpeed, float landingKnockBack)
    {
        Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);

        while (!IsGrounded())
        {
            RigidBody.velocity = new Vector2(0, stompSpeed);
            yield return null;
        }
        RigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.05f);
        Instantiate(stompParticle, transform.position, Quaternion.identity);
        DealDamageTo(DetectEnemiesInRange(areaOfEffect), landingKnockBack);
        Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
    }

    public void ProcessEnemies(Collider2D[] detectedEntities, Action<EnemyScript> action)
    {
        foreach (Collider2D enemyCollider in detectedEntities)
        {
            if (enemyCollider.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                action?.Invoke(enemyScript);
            }
        }
    }

    public void AttackMoveAllEnemiesHit(Collider2D[] detectedEntities, int moveBy)
    {
        ProcessEnemies(detectedEntities, enemyScript => enemyScript.AttackMoveMe(moveBy, lastDirection));
    }

    public void LiftUpAllEnemiesHit(Collider2D[] detectedEntities, int liftByThisMuch)
    {
        ProcessEnemies(detectedEntities, enemyScript => enemyScript.LiftMeUp(liftByThisMuch));
    }

    public void StompDownAllEnemiesHit(Collider2D[] detectedEntities, int stompSpeed)
    {
        ProcessEnemies(detectedEntities, enemyScript => enemyScript.StompMeDown(stompSpeed));
    }

    void OnDrawGizmosSelected()
    {
        if (playerBase.attackPoint.position == null) { return; }
        Gizmos.DrawWireSphere(playerBase.attackPoint.position, MEELE_ATTACK_RANGE);
    }

    private Collider2D[] DetectEnemiesInRange(float range)
    {
        return Utility.DetectByLayers(playerBase.attackPoint.position, range, playerBase.enemyLayers);
    }
}