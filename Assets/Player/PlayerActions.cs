using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public partial class PlayerScript 
{
    public Transform attackPoint;
    public float attackRange = 100.0f;
    public GameObject bulletPrefab;
    public BoxCollider2D collision;
    public LayerMask enemyLayers;
    public LayerMask buildingLayers;
    public LayerMask groundLayers;
    public LineRenderer laser;
    public ParticleSystem suicideParticleEffect;
    public Transform groundCheck;

    private void MeeleAttack()
    {

        DetectAndDealDamage(attackPoint.position, attackRange);
    }

    private void StompAttack()
    {
        float areaOfEffect = 5;
        if (!IsGrounded())
        {
            StartCoroutine(StompDown(areaOfEffect));
            return;
        }

        Instantiate(suicideParticleEffect, transform.position, Quaternion.identity);
        DetectAndDealDamage(transform.position, areaOfEffect);
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
        DetectAndDealDamage(transform.position, areaOfEffect);
        DisableEnemyCollision(false);
    }


    private void Build()
    {
        Collider2D[] hitBuildings = Physics2D.OverlapCircleAll(gameObject.transform.position, 1, buildingLayers);

        foreach (Collider2D building in hitBuildings)
        {
            BuildingScript buildingScript = building.GetComponent<BuildingScript>();

            if (buildingScript != null)
            {
                buildingScript.Build();
            }
        }
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

    private void DetectAndDealDamage(Vector2 hitPosition, float areaOfEffect)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPosition, areaOfEffect, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
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
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}