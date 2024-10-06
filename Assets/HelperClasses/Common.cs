using System.Collections;
using UnityEngine;

public static class Common
{
    public static IEnumerator LiftUp(int liftByThisMuch, float positionBeforeLiftUpY, Rigidbody2D rigidBody, UnityEngine.Transform transform)
    {
        float elapsedTime = 0f;
        TurnOffGravity(rigidBody, true);

        while (positionBeforeLiftUpY + liftByThisMuch > transform.position.y && elapsedTime <= 0.25f)
        {
            elapsedTime += Time.deltaTime;
            rigidBody.velocity = new Vector2(0, 30);
            yield return null;
        }
        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        TurnOffGravity(rigidBody, false);
    }

    public static IEnumerator WarriorMoveAttack(float positionBeforeMoveX, int moveBy, float moveToDirection, UnityEngine.Transform transform, Rigidbody2D rigidBody)
    {
        float elapsedTime = 0f;
        TurnOffGravity(rigidBody, true);

        if (moveToDirection > 0)
        {
            while (positionBeforeMoveX + moveBy > transform.position.x && elapsedTime < 0.3f)
            {
                elapsedTime += Time.deltaTime;
                rigidBody.velocity = new Vector2(15, 0);
                yield return null;
            }
        }
        else
        {
            while (positionBeforeMoveX - moveBy < transform.position.x && elapsedTime < 0.3f)
            {
                elapsedTime += Time.deltaTime;
                rigidBody.velocity = new Vector2(-15, 0);
                yield return null;
            }
        }
        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        TurnOffGravity(rigidBody, false);
    }

    public static void TurnOffGravity(Rigidbody2D rigidBody, bool turnOffGravity)
    {
        const int DEFAULT_GRAVITY = 8;
        rigidBody.gravityScale = turnOffGravity ? 0 : DEFAULT_GRAVITY;
    }
}