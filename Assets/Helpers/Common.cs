using System.Collections;
using Living;
using UnityEngine;

namespace Helpers
{
    public static class Common
    {
        public static IEnumerator LiftUp(int liftByThisMuch, float positionBeforeLiftUpY, Rigidbody2D rigidBody, Transform transform, LivingEntity affectedEntity)
        {
            affectedEntity?.Immobilize(true);
            float elapsedTime = 0f;
            TurnOffGravity(rigidBody, true);

            while (positionBeforeLiftUpY + liftByThisMuch > transform.position.y && elapsedTime <= 0.25f)
            {
                elapsedTime += Time.deltaTime;
                rigidBody.linearVelocity = new Vector2(0, 30);
                yield return null;
            }
            rigidBody.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            TurnOffGravity(rigidBody, false);
        }

        public static IEnumerator WarriorMoveAttack(float positionBeforeMoveX, float moveBy, float moveToDirection, Transform transform, Rigidbody2D rigidBody, LivingEntity affectedEntity)
        {
            affectedEntity?.Immobilize(true);
            float elapsedTime = 0f;
            TurnOffGravity(rigidBody, true);

            if (moveToDirection > 0)
            {
                while (positionBeforeMoveX + moveBy > transform.position.x && elapsedTime < 0.3f)
                {
                    elapsedTime += Time.deltaTime;
                    rigidBody.linearVelocity = new Vector2(15, 0);
                    yield return null;
                }
            }
            else
            {
                while (positionBeforeMoveX - moveBy < transform.position.x && elapsedTime < 0.3f)
                {
                    elapsedTime += Time.deltaTime;
                    rigidBody.linearVelocity = new Vector2(-15, 0);
                    yield return null;
                }
            }
            rigidBody.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(0.35f);
            TurnOffGravity(rigidBody, false);
        }

        public static void TurnOffGravity(Rigidbody2D rigidBody, bool turnOffGravity)
        {
            const int DEFAULT_GRAVITY = 8;
            rigidBody.gravityScale = turnOffGravity ? 0 : DEFAULT_GRAVITY;
        }
    }
}