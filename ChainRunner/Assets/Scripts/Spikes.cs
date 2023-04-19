using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // public float kbX = -10f;
    // public float kbY = 20f;
    public float damage = 10f;
    public float kbForce = -20f;
    private float timeLeft = 0;
    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && timeLeft <= 0) {
            Vector2 dir = (transform.position - other.transform.position).normalized;
            PlayerController.p.KnockBack(dir.normalized, kbForce);
            PlayerController.p.TakeDamage(damage);
            timeLeft = .2f;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && timeLeft <= 0)
        {
            Vector2 dir = (transform.position - other.transform.position).normalized;
            PlayerController.p.KnockBack(dir.normalized, kbForce);
            PlayerController.p.TakeDamage(damage);
            timeLeft = .2f;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && timeLeft <= 0)
        {
            Vector2 dir = (transform.position - other.transform.position).normalized;
            PlayerController.p.KnockBack(dir.normalized, kbForce);
            PlayerController.p.TakeDamage(damage);
            timeLeft = .2f;
        }
    }

}
