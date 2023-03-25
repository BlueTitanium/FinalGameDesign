using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player has been attacked");
            PlayerController.p.TakeDamage(enemyController.attackDamage);
        }
    }
}
