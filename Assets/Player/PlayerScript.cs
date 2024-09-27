using UnityEngine;

public partial class PlayerScript : LivingEntity
{
    private void Start()
    {
        Init(_health: 100,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        if (!dashing)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); // A (-1) and D (+1)

            if (Input.GetKeyDown("c"))
            {
                MeeleAttack();
            }

            if (Input.GetKeyDown("e"))
            {
                PushBack();
            }

            if (Input.GetKeyDown("q"))
            {
                StompAttack();
            }

            if (Input.GetKeyDown("b"))
            {
                Build();
            }

            if (Input.GetKeyDown("x"))
            {
                BulletAttack();
            }

            if (Input.GetKeyDown("r"))
            {
                CommitSuicide();
            }

            if (Input.GetKeyDown("z"))
            {
                StartCoroutine(LaserAttack());
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void FixedUpdate()
    {
        if (movement.x != 0)
        {
            Move();
        }
    }
}