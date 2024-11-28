using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class PersonaAbstract : MonoBehaviour, PersonaInterface
{
    protected PlayerBase playerBase;
    public abstract string PersonaName { get; set; }
    public virtual int maxNumberOfJumps => 5;

    private float moveSpeed = 10f;
    private float jumpForce = 20f;
    protected float dashForce = 100f;
    private Vector2 movement;
    //public bool dashing = false;

    //TODO: tohle nesmi byt static, jinak nebude multiplier fungovat vubec.
    protected static float lastDirection = 1;
    private static int consecutiveJumps = 1;
    protected Rigidbody2D RigidBody;

    [SerializeField]
    private Sprite skin;

    public void Initialize(PlayerBase _playerBase)
    {
        playerBase = _playerBase;
        RigidBody = playerBase.GetRigidBody();
    }

    protected void RunMovementCoroutine(IEnumerator coroutine)
    {
        StopMovementCoroutine();
        playerBase.movementCoroutine = StartCoroutine(coroutine);
    }

    protected void StopMovementCoroutine()
    {
        if (playerBase.movementCoroutine != null)
        {
            StopCoroutine(playerBase.movementCoroutine);
        }
    }

    protected bool IsGrounded()
    {
        return Utility.IsGroundedOnLayers(playerBase.groundCheck.position, playerBase.groundLayers);
    }

    public virtual void Dash()
    {
        RunMovementCoroutine(DashCoroutine());
    }

    protected virtual IEnumerator DashCoroutine()
    {
        ResetJumps();
        Utility.IgnoreCollisionsByLayers(true, gameObject.layer, playerBase.enemyLayers);
        Common.TurnOffGravity(RigidBody, true);
        //dashing = true;
        RigidBody.velocity = new Vector2(lastDirection * dashForce, 0);
        yield return new WaitForSeconds(0.1f);
        RigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        //dashing = false;
        Common.TurnOffGravity(RigidBody, false);
        Utility.IgnoreCollisionsByLayers(false, gameObject.layer, playerBase.enemyLayers);
    }

    protected void ResetJumps()
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
        Collider2D closestBuilding = Utility.DetectByLayers(transform.position, 1, playerBase.buildingLayers)
            .FirstOrDefault();

        closestBuilding?.GetComponent<BuildingScript>().Build();
    }

    public void DoDialogWithNPC()
    {
        Collider2D closestNpc = Utility.DetectByLayers(transform.position, 5, playerBase.npcLayers)
            .FirstOrDefault();

        closestNpc?.GetComponent<NpcScript>().DoDialog();
    }

    public void CommitSuicide()
    {
        playerBase.TakeDamage(playerBase.GetCurrentHealth());
    }

    public void DoDialog()
    {
        DialogManager.Instance.PopUpDialog("Ambatakaaaaaam", gameObject.transform.position);
    }

    private void BulletAttack()
    {
        Debug.Log("pew");
        //Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
    }

    public void Jump(int maxJumps = 5)
    {
        if (IsGrounded())
        {
            ResetJumps();
        }
        else if (!IsGrounded() && maxNumberOfJumps <= consecutiveJumps)
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

    protected bool DealDamageTo(Collider2D[] detectedEnemies, float knockback)
    {
        int count = 0;
        foreach (Collider2D enemy in detectedEnemies)
        {
            if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                enemyScript.TakeDamage(10);
                enemyScript.GetKnockedBack(this.transform.position, knockback);
                count++;
                Debug.Log("hit enemy");
            }
        }
        return count > 0;
    }

    protected Collider2D[] DetectEnemiesInRange(float range)
    {
        return Utility.DetectByLayers(playerBase.attackPoint.position, range, playerBase.enemyLayers);
    }

    protected void ProcessEnemies(Collider2D[] detectedEntities, Action<EnemyScript> action)
    {
        foreach (Collider2D enemyCollider in detectedEntities)
        {
            if (enemyCollider.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                action?.Invoke(enemyScript);
            }
        }
    }

    public abstract void BaseAttack();

    public abstract void FirstAbility();

    public abstract void SecondAbility();

    public abstract void SwapToMe();

    public abstract void SwapFromMe();

}