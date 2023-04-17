using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform toLocation;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController.p.transform.position = new Vector2(toLocation.position.x, PlayerController.p.transform.position.y);
        } else if (other.CompareTag("Enemy")) {
            other.transform.root.position = new Vector2(toLocation.position.x, other.transform.root.position.y);
        }
    }
}
