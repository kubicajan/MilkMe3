using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 100.0f;
    public GameObject bulletPrefab;
    public BoxCollider2D collision;
    public LayerMask enemyLayers;
    public LayerMask buildingLayers;
    public LineRenderer laser;

    private void MeeleAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(10);
                Debug.Log("hit enemy");
            }
        }
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
        gameObject.SetActive(false);
    }


    private IEnumerator LaserAttack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.right, Mathf.Infinity, enemyLayers);

        if (hitInfo)
        {
            EnemyScript enemy = hitInfo.transform.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                Debug.Log("hit enemy");
                enemy.TakeDamage(10);
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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}