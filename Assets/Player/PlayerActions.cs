using System.Collections;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public partial class PlayerScript
{
    public Transform attackPoint;
    public GameObject bulletPrefab;
    public BoxCollider2D collision;
    public LayerMask enemyLayers;
    public LayerMask buildingLayers;
    public LayerMask groundLayers;
    public LineRenderer laser;
    public ParticleSystem suicideParticleEffect;
    public Transform groundCheck;
    private const float MEELE_ATTACK_RANGE = 2f;

    private void MeeleAttack()
    {
        DealDamageTo(Detect(attackPoint.position, MEELE_ATTACK_RANGE, enemyLayers));
    }

    private void StompAttack()
    {
        const float AREA_OF_EFFECT = 5f;
        if (!IsGrounded())
        {
            StartCoroutine(StompDown(AREA_OF_EFFECT));
            return;
        }

        Instantiate(suicideParticleEffect, transform.position, Quaternion.identity);
        DealDamageTo(Detect(transform.position, AREA_OF_EFFECT, enemyLayers));
    }

    private IEnumerator StompDown(float areaOfEffect)
    {
        DisableEnemyCollision(true);
        rigidBody.velocity = new Vector2(0, -100);

        while (!IsGrounded())
        {
            yield return null;
        }
        rigidBody.velocity = new Vector2();
        Instantiate(suicideParticleEffect, transform.position, Quaternion.identity);
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
        Instantiate(suicideParticleEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
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
                enemyScript.GetKnockbacked(this.transform.position);
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
                enemyScript.GetKnockbacked(this.transform.position);
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