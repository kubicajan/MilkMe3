using UnityEngine;

public partial class PlayerController : LivingEntity
{
    private Farmer farmer;
    private Mage mage;
    private PersonaAbstract currentPersona;

    private void Start()
    {
        farmer = GetComponent<Farmer>();
        mage = GetComponent<Mage>();
        currentPersona = farmer;

        Init(_health: 100,
            _rigidBody2D: GetComponent<Rigidbody2D>(),
            _boxCollider: GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        if (!dashing)
        {
            if (Input.GetKeyDown("o"))
            {
                if (currentPersona != farmer)
                {
                    currentPersona = farmer;
                }
                else
                {
                    currentPersona = mage;
                }
                Debug.Log("switched to" + currentPersona.PersonaName);
            }

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