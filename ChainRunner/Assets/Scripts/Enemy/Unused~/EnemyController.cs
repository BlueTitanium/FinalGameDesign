using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHP = 100;
    private float currHP; // initialize in Start()

    [Header("Movement")]
    public bool isFacingRight = true;

    [Header("Attack")]
    [SerializeField] private Transform lineOfSight;
    [SerializeField] private float lineOfSightDistance = 5f;
    public float attackDamage = 5f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    float attackTimer;
    bool isAttackCooldown = false;
    [HideInInspector] public bool playerDetected = false;
    float playerDistance; // distance between self and players

    [Header("Colliders")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private float ledgeCastDist = 0.75f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask enemyLayer;


    private Rigidbody2D rb;
    private Transform playerTransform;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        attackTimer = attackCooldown;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerDistance = Vector2.Distance(transform.position, playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(lineOfSight.position, transform.right, 
            (isFacingRight) ? lineOfSightDistance : -lineOfSightDistance, ~enemyLayer);
        Debug.DrawRay(lineOfSight.position, transform.right * ((isFacingRight) ? lineOfSightDistance : -lineOfSightDistance), 
            (playerDistance > attackDistance) ? Color.red : Color.green);

        if (hit.collider != null && hit.collider.CompareTag("Player")) {
            playerDetected = true;
        } 

        if (playerDetected) {
            playerDistance = Vector2.Distance(transform.position, playerTransform.position);
            animator.SetBool("PlayerDetected", true);

            if (playerDistance > attackDistance) {
                // chase
                animator.SetBool("PlayerInAttackRange", false);
                animator.SetBool("CanAttack", false);
            } else if (attackDistance >= playerDistance) {
                animator.SetBool("PlayerInAttackRange", true);
                // attack
                animator.SetBool("CanAttack", !isAttackCooldown);
            }
        } else {
            animator.SetBool("PlayerDetected", false);
            animator.SetBool("CanAttack", false);
        }

        if (isAttackCooldown) {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0 && isAttackCooldown) {
                isAttackCooldown = false;
                attackTimer = attackCooldown;
            }
        }
    }

    public void TakeDamage(float dmg) {
        currHP -= dmg;

        if (currHP <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }

    public void flipX() {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public bool isHittingWall() {
        float castDist = (isFacingRight) ? ledgeCastDist : -ledgeCastDist;

        Vector3 targetPos = wallCheck.position;
        targetPos.x += castDist;

        Debug.DrawLine(wallCheck.position, targetPos, Color.red);

        return Physics2D.Linecast(wallCheck.position, targetPos, groundLayer | wallLayer);
    }

    public bool isNearEdge() {
        Vector3 targetPos = ledgeCheck.position;
        targetPos.y -= ledgeCastDist;

        Debug.DrawLine(ledgeCheck.position, targetPos, Color.red);

        return !Physics2D.Linecast(ledgeCheck.position, targetPos, groundLayer | wallLayer);
    }

    public bool PlayerInAttackRange() {
        return attackDistance >= playerDistance;
    }

    public void LookAtPlayer() {
        if ((playerTransform.position.x > transform.position.x && !isFacingRight) ||
            (playerTransform.position.x < transform.position.x && isFacingRight)) {
            flipX();
        }
    }


    // call this at last frame of attack animation
    public void StartAttackCooldown() {
        isAttackCooldown = true;
    }
}
