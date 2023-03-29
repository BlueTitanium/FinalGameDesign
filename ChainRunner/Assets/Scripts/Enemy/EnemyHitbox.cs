using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Vector2 dir = (transform.position - other.transform.position).normalized;
            PlayerController.p.TakeDamage(enemy.attackDamage);
            PlayerController.p.KnockBack(-dir, 5);
        }
    }
}
