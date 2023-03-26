using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHP = 100;
    protected float currHP; // initialize in Start()

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected bool isFacingRight = true;

    [Header("Attack")]
    public float attackDamage = 5f;
    [HideInInspector] public bool playerDetected = false;

    protected Rigidbody2D rb;
    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void TakeDamage(float dmg) {
        currHP -= dmg;
        playerDetected = true;

        if (currHP <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetTrigger("Death");
        StopAllCoroutines();
        // Destroy(gameObject);
    }
}
