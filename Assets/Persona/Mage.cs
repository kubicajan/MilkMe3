
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mage : PersonaAbstract
{
    public override string PersonaName { get; set; } = "Mage";

    public override void BaseAttack()
    {
        Debug.Log("Unfinished");
        return;
    }

    public override void FirstAttack()
    {
        PushBack();
    }

    public override void SecondAttack()
    {
        Debug.Log("Unfinished");
        return;
    }


    private void PushBack()
    {
        const float DISTANCE_LIMIT = 10f;
        Collider2D[] enemiestDetected = Utility.DetectByLayers(gameObject.transform.position, DISTANCE_LIMIT / 2, playerBase.enemyLayers);
        foreach (Collider2D enemy in enemiestDetected)
        {
            if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                float currentDistance = Vector2.Distance(enemy.transform.position, transform.position);

                StartCoroutine(enemyScript.MoveEnemyCoroutine(this.transform.position, DISTANCE_LIMIT));
                Debug.Log("enemy pushed");
            }
        }
    }
}