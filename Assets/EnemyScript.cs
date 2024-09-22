using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{health} health remaining");
    }
}
