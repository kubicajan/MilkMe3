using System.Collections;
using UnityEngine;


public static class Common
{
    public static IEnumerator LiftUp(int liftByThisMuch, float positionBeforeLiftUpY, Rigidbody2D rigidBody, Transform transform)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
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

    public static void TurnOffGravity(Rigidbody2D rigidBody, bool turnOffGravity)
    {
        const int DEFAULT_GRAVITY = 8;
        rigidBody.gravityScale = turnOffGravity ? 0 : DEFAULT_GRAVITY;
    }
}