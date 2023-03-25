using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZone : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    void OnTriggerExit2D(Collider2D other) {
        enemyController.playerDetected = false;
        enemyController.SwitchToState(EnemyController.State.Patrol);
    }
}
