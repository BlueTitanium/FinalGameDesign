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
    [SerializeField] protected bool canBeStunned = true;
    protected bool isStunned = false;
    [SerializeField] protected bool canBeKnockbacked = true;
    protected bool knockbacked;

    [Header("Sprite")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Coroutine damageFlashCoroutine;
    protected Coroutine stunCoroutine;
    [SerializeField] protected GameObject dizzyEffect;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform playerTransform;

    [Header("Others")]
    [SerializeField] private bool unkillable; // "takes damage", but doesn't actually decrease HP


    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void flipX() {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public virtual void TakeDamage(float dmg, bool showsDamageAnimation = true) {
        AudioManager.PlaySound("enemyHurt");
        dmg *= PlayerController.p.damageMultiplier;
        if (!unkillable) currHP -= dmg;
        playerDetected = true;
        DmgTextController.d.SpawnDmgText(dmg, transform.position);
        if (showsDamageAnimation)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetTrigger("Hurt");
            }

            DamageFlash();
        }
        if (currHP <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetTrigger("Death");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public virtual void Stun(float duration) {
        if (!canBeStunned) return;
        
        if (stunCoroutine != null) {
            StopCoroutine(stunCoroutine);
        }
        
        stunCoroutine = StartCoroutine(StunIE(duration));
    }

    protected virtual IEnumerator StunIE(float duration) {
        isStunned = true;
        animator.SetBool("isStunned", true);
        if (dizzyEffect != null) dizzyEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        isStunned = false;
        animator.SetBool("isStunned", false);
        if (dizzyEffect != null) dizzyEffect.SetActive(false);
    }

    protected virtual void DamageFlash() {
        if (damageFlashCoroutine != null) {
            StopCoroutine(damageFlashCoroutine);
        }

        damageFlashCoroutine = StartCoroutine(DamageFlashIE());
    }

    IEnumerator DamageFlashIE() {
        spriteRenderer.color = Color.red;
        CameraShake.cs.cameraShake(.3f, 1.6f);
        while (spriteRenderer.color != Color.white) {
            yield return null;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, Time.deltaTime);
        }

        // yield return new WaitForSeconds(0.2f);
        // spriteRenderer.color = Color.white;
    }

    public virtual void Knockback(Transform t, float knockbackForce) {
        if (!canBeKnockbacked) return;

        Vector2 dir = transform.position - t.position;
        StartCoroutine(KnockbackTimer(dir, knockbackForce));
    }

    IEnumerator KnockbackTimer(Vector2 dir, float knockbackForce) {
        knockbacked = true;
        yield return null;
        // rb.velocity = dir.normalized * knockbackForce;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir.normalized * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        knockbacked = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    protected void LookAtPlayer() {
        if ((playerTransform.position.x > transform.position.x && !isFacingRight) ||
            (playerTransform.position.x < transform.position.x && isFacingRight)) {
            flipX();
        }
    }

    protected void FaceMovementDirection() {
        if ((rb.velocity.x > 0.05 && !isFacingRight) || 
            (rb.velocity.x < -0.05 && isFacingRight)) flipX();
    }
}
