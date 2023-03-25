using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // public float kbX = -10f;
    // public float kbY = 20f;
    public float kbForce = -20f;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Vector2 dir = (transform.position - other.transform.position).normalized;
            GameObject.FindObjectOfType<PlayerController>().KnockBack(dir, kbForce);
            //GameObject.FindObjectOfType<PlayerController>().KnockBack(dir, kbX, kbY);
        }
    }

}
