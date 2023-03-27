using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : Enemy
{
    [Header("Melee Movement")]
    [SerializeField] private float chaseSpeed = 2f;

    [Header("Melee Attack")]
    [SerializeField] private Transform lineOfSight;
    [SerializeField] private float lineOfSightDistance = 5f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    float attackTimer;
    bool isAttackCooldown = false;
    float playerDistance; // distance between self and player

    [Header("Melee States")]
    [SerializeField] private State initialState = State.Idle;
    private State currentState;
    [HideInInspector] public enum State {Idle, Patrol, Chase, Attack}


    [Header("Melee Colliders")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private float ledgeCastDist = 0.75f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask enemyLayer;


    private Transform playerTransform;


    [Header("Others")]
    [SerializeField] EnemyHotzone hotzone;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        attackTimer = attackCooldown;
        SwitchToState(initialState);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerDistance = Vector2.Distance(transform.position, playerTransform.position);

        hotzone.PlayerExitedCallback += PatrolState;
    }

    void OnDestroy() {
        hotzone.PlayerExitedCallback -= PatrolState;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) TakeDamage(10);
        if (currHP <= 0) return;

        RaycastHit2D hit = Physics2D.Raycast(lineOfSight.position, transform.right, 
            (isFacingRight) ? lineOfSightDistance : -lineOfSightDistance, ~enemyLayer);
        Debug.DrawRay(lineOfSight.position, transform.right * ((isFacingRight) ? lineOfSightDistance : -lineOfSightDistance), 
            (playerDistance > attackDistance) ? Color.red : Color.green);

        if (hit.collider != null && hit.collider.CompareTag("Player")) {
            playerDetected = true;
        } 

        if (playerDetected) {
            playerDistance = Vector2.Distance(transform.position, playerTransform.position);

            if (playerDistance > attackDistance) {
                SwitchToState(State.Chase);
            } else if (attackDistance >= playerDistance && !isAttackCooldown) {
                SwitchToState(State.Attack);
            } else if (attackDistance >= playerDistance && isAttackCooldown) {
                SwitchToState(State.Idle);
            }
        }

        if (isAttackCooldown) {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0 && isAttackCooldown) {
                isAttackCooldown = false;
                attackTimer = attackCooldown;
            }
        }
    }

    void FixedUpdate() {
        if (currHP <= 0) {
            return;
        }
        
        // state behaviors
        switch (currentState) {
            case State.Idle:
                animator.SetTrigger("Idle");
                break;
            case State.Patrol:
                animator.SetBool("canAttack", false);
                animator.SetTrigger("Walk");
                break;
            case State.Chase:
                animator.SetBool("canAttack", false);
                animator.SetTrigger("Walk");
                break;
            case State.Attack:
                animator.SetBool("canAttack", !isAttackCooldown);
                break;
            default:
                Debug.Log("Current state not in switch-case");
                break;
        }

        SetMovement();
    }


    bool isHittingWall() {
        float castDist = (isFacingRight) ? ledgeCastDist : -ledgeCastDist;

        Vector3 targetPos = wallCheck.position;
        targetPos.x += castDist;

        Debug.DrawLine(wallCheck.position, targetPos, Color.red);

        return Physics2D.Linecast(wallCheck.position, targetPos, groundLayer | wallLayer);
    }

    bool isNearEdge() {
        Vector3 targetPos = ledgeCheck.position;
        targetPos.y -= ledgeCastDist;

        Debug.DrawLine(ledgeCheck.position, targetPos, Color.red);

        return !Physics2D.Linecast(ledgeCheck.position, targetPos, groundLayer | wallLayer);
    }

    void LookAtPlayer() {
        if ((playerTransform.position.x > transform.position.x && !isFacingRight) ||
            (playerTransform.position.x < transform.position.x && isFacingRight)) {
            flipX();
        }
    }


    // call this at last frame of attack animation
    public void StartAttackCooldown() {
        isAttackCooldown = true;
    }

    public void PatrolState() {
        SwitchToState(MeleeEnemyController.State.Patrol);
    }

    void SetMovement() {
        // state movement behavior
        if (knockbacked) {
            float lerpedXVelocity = Mathf.Lerp(rb.velocity.x, 0f, Time.fixedDeltaTime);
            rb.velocity = new Vector2(lerpedXVelocity, rb.velocity.y);
            return;
        }

        switch (currentState) {
            case State.Idle:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case State.Patrol:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                    float vX = (isFacingRight) ? moveSpeed : -moveSpeed;

                    rb.velocity = new Vector2(vX, rb.velocity.y);    

                    if (isHittingWall() || isNearEdge()) {
                        flipX();
                    }
                }
                break;
            case State.Chase:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                    // Vector2 targetPos = new Vector2(playerTransform.position.x, rb.position.y);
                    // transform.position = Vector2.MoveTowards(rb.position, targetPos, chaseSpeed * Time.fixedDeltaTime);
                    if (attackDistance >= playerDistance) {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (rb.position.x < playerTransform.position.x) {
                        rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
                    }
                    else if (rb.position.x > playerTransform.position.x) {
                        rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
                    }

                    LookAtPlayer();
                }

                break;
            case State.Attack:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            default:
                Debug.Log("Current state not in switch-case");
                break;
        }
    }


    private Coroutine ChangeState;
    public void SwitchToState(State newState) {
        // handle on state enters and exits
        if (currentState != newState) {
            // on state leave
            switch (currentState) {
                case State.Idle:
                    break;
                case State.Attack:
                    break;
                default:
                    break;
            }
            if (ChangeState != null) StopCoroutine(ChangeState);

            // set new state
            currentState = newState;

            // on state enters
            switch (newState) {
                case State.Idle:
                    float idle_seconds = Random.Range(2f, 4f);
                    ChangeState = StartCoroutine(SwitchToStateIE(State.Patrol, idle_seconds));
                    break;
                case State.Patrol:
                    if (Random.value > 0.5f) flipX(); // choose new direction to patrol
                    float patrol_seconds = Random.Range(5f, 10f);
                    ChangeState = StartCoroutine(SwitchToStateIE(State.Idle, patrol_seconds));
                    break;
                case State.Attack:
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator SwitchToStateIE(State newState, float seconds) {
        yield return new WaitForSeconds(seconds);
        SwitchToState(newState);
    }
}
