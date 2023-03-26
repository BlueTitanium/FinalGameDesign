using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHotzone : MonoBehaviour
{
    [SerializeField] Enemy enemyController;
    public delegate void OnPlayerExited();
    public OnPlayerExited PlayerExitedCallback;

    void OnTriggerExit2D(Collider2D other) {
        if (enemyController != null && other.CompareTag("Player")) {
            enemyController.playerDetected = false;
            PlayerExitedCallback?.Invoke();
        }
    }
}

