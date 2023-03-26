using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox_ : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController.p.TakeDamage(enemyController.attackDamage);
        }
    }
}
