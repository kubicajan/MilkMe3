using System.Collections;
using System.Linq;
using UnityEngine;

public partial class PlayerController
{
    public LineRenderer laser;
    public GameObject bulletPrefab;
    public Transform attackPoint;
    public Transform groundCheck;
    public LayerMask enemyLayers;
    public LayerMask groundLayers;
    public LayerMask buildingLayers;

    private const float MEELE_ATTACK_RANGE = 2f;

    private void MeeleAttack()
    {
        DealDamageTo(Detect(attackPoint.position, MEELE_ATTACK_RANGE, enemyLayers));
    }

    private void PushBack()
    {
        const float DISTANCE_LIMIT = 10f;
        Collider2D[] enemiestDetected = Detect(gameObject.transform.position, DISTANCE_LIMIT/2, enemyLayers);
        foreach (Collider2D enemy in enemiestDetected)
        {
            if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                float currentDistance = Vector2.Distance(enemy.transform.position, transform.position);

                StartCoroutine(enemyScript.MoveEnemyCoroutine(this.transform.position, DISTANCE_LIMIT));
                Debug.Log("enemy pushed");
            }
        }
    }

    private void StompAttack()
    {
        const float AREA_OF_EFFECT = 5f;
        if (!IsGrounded())
        {
            StartCoroutine(StompDown(AREA_OF_EFFECT));
            return;
        }

        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        DealDamageTo(Detect(transform.position, AREA_OF_EFFECT, enemyLayers));
    }

    private IEnumerator StompDown(float areaOfEffect)
    {
        DisableEnemyCollision(true);
        RigidBody.velocity = new Vector2(0, -100);

        while (!IsGrounded())
        {
            yield return null;
        }
        RigidBody.velocity = new Vector2();
        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        DealDamageTo(Detect(transform.position, areaOfEffect, enemyLayers));
        DisableEnemyCollision(false);
    }

    private void Build()
    {
        BuildingScript hitBuilding = Detect(transform.position, 1, buildingLayers)
            .First()
            .GetComponent<BuildingScript>();
        hitBuilding.Build();
    }

    private void BulletAttack()
    {
        Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
    }

    private void CommitSuicide()
    {
        TakeDamage(GetCurrentHealth());
    }

    private IEnumerator LaserAttack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.right, Mathf.Infinity, enemyLayers);

        if (hitInfo)
        {
            EnemyScript enemyScript = hitInfo.transform.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                Debug.Log("hit enemy");
                enemyScript.TakeDamage(10);
                enemyScript.GetKnockedBack(this.transform.position, 0.5f);
                laser.SetPosition(0, attackPoint.position);
                laser.SetPosition(1, hitInfo.point);
            }
        }
        else
        {
            laser.SetPosition(0, attackPoint.position);
            laser.SetPosition(1, new Vector2(lastDirection * 200, attackPoint.position.y));
        }
        laser.enabled = true;
        yield return new WaitForSeconds(0.1f);
        laser.enabled = false;
    }

    private Collider2D[] Detect(Vector2 hitPosition, float areaOfEffect, LayerMask layersToBeDetected)
    {
        return Physics2D.OverlapCircleAll(hitPosition, areaOfEffect, layersToBeDetected);
    }

    private void DealDamageTo(Collider2D[] detectedEnemies)
    {
        foreach (Collider2D enemy in detectedEnemies)
        {
            if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                enemyScript.TakeDamage(10);
                enemyScript.GetKnockedBack(this.transform.position, 2);
                Debug.Log("hit enemy");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, MEELE_ATTACK_RANGE);
    }
}