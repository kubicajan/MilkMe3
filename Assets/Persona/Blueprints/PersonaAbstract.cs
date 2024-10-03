using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class PersonaAbstract : MonoBehaviour, PersonaInterface
{

    protected PlayerBase playerBase;
    public abstract string PersonaName { get; set; }

    private float moveSpeed = 10f;
    private float jumpForce = 20f;
    private float dashForce = 100f;
    private Vector2 movement;
    //public bool dashing = false;
    private float lastDirection = 1;
    private int consecutiveJumps = 1;
    protected Rigidbody2D RigidBody;

    [SerializeField]
    private Sprite skin;

    public void Initialize(PlayerBase _playerBase)
    {
        playerBase = _playerBase;
        RigidBody = playerBase.GetRigidBody();
    }
    protected bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerBase.groundCheck.position, 0.2f, playerBase.groundLayers);
        bool isGrounded = colliders.Length > 0;
        return isGrounded;
    }

    public IEnumerator Dash()
    {
        ResetJumps();
        Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);
        float originalGravity = RigidBody.gravityScale;
        //dashing = true;
        RigidBody.gravityScale = 0;
        RigidBody.velocity = new Vector2(lastDirection * dashForce, 0);
        yield return new WaitForSeconds(0.1f);
        RigidBody.velocity = new Vector2();
        yield return new WaitForSeconds(0.1f);
        //dashing = false;
        RigidBody.gravityScale = originalGravity;
        Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
    }

    private void ResetJumps()
    {
        consecutiveJumps = 0;
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public void Move()
    {
        if (lastDirection != movement.x)
        {
            lastDirection = movement.x;
            transform.Rotate(0f, 180f, 0f);
        }

        Vector3 m_Velocity = Vector3.zero;
        Vector3 targetVelocity = new Vector2(movement.x * moveSpeed, RigidBody.velocity.y);
        RigidBody.velocity = Vector3.SmoothDamp(RigidBody.velocity, targetVelocity, ref m_Velocity, .05f);
    }

    public void MovePotentially()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // A (-1) and D (+1)
    }

    public void Build()
    {
        BuildingScript hitBuilding = Utility.DetectByLayers(transform.position, 1, playerBase.buildingLayers)
            .First()
            .GetComponent<BuildingScript>();
        hitBuilding.Build();
    }

    public void CommitSuicide()
    {
        playerBase.TakeDamage(playerBase.GetCurrentHealth());
    }

    private void BulletAttack()
    {
        Debug.Log("pew");
        //Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
    }

    private IEnumerator LaserAttack()
    {
        Debug.Log("laser pew");
        yield return null;
        //RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.right, Mathf.Infinity, enemyLayers);

        //if (hitInfo)
        //{
        //    EnemyScript enemyScript = hitInfo.transform.GetComponent<EnemyScript>();
        //    if (enemyScript != null)
        //    {
        //        Debug.Log("hit enemy");
        //        enemyScript.TakeDamage(10);
        //        enemyScript.GetKnockedBack(this.transform.position, 0.5f);
        //        laser.SetPosition(0, attackPoint.position);
        //        laser.SetPosition(1, hitInfo.point);
        //    }
        //}
        //else
        //{
        //    laser.SetPosition(0, attackPoint.position);
        //    laser.SetPosition(1, new Vector2(lastDirection * 200, attackPoint.position.y));
        //}
        //laser.enabled = true;
        //yield return new WaitForSeconds(0.1f);
        //laser.enabled = false;
    }

    public void Jump()
    {
        const int MAX_JUMPS = 5;
        if (IsGrounded())
        {
            ResetJumps();
        }
        else if (!IsGrounded() && MAX_JUMPS <= consecutiveJumps)
        {
            return;
        }

        consecutiveJumps++;
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);
    }



    public Sprite GetSkin()
    {
        return skin;
    }

    protected void DealDamageTo(Collider2D[] detectedEnemies)
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

    public abstract void BaseAttack();

    public abstract void FirstAttack();

    public abstract void SecondAttack();
}