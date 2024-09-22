using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    private void Update()
    {
        if (!dashing)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); // A (-1) and D (+1)

            if (Input.GetKeyDown("c"))
            {
                MeeleAttack();
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