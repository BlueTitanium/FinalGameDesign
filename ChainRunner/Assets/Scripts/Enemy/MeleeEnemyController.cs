using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeEnemyController : Enemy
{
    [Header("Chase Speed")]
    [SerializeField] private float chaseSpeed = 2f;

    [Header("Attack Stats")]
    // [SerializeField] private Transform lineOfSight;
    // [SerializeField] private float lineOfSightDistance = 5f;
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    float attackTimer;
    protected bool isAttackCooldown = false;
    protected float playerDistance; // distance between self and player

    [Header("States")]
    [SerializeField] protected State initialState = State.Idle;
    protected State currentState;
    [HideInInspector] public enum State {Idle, Patrol, Chase, Attack, Surround}


    [Header("Colliders")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform backWallCheck;
    [SerializeField] private Transform backLedgeCheck;
    [SerializeField] private Transform jumpCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float wallCastDist = 0.01f;
    [SerializeField] private float ledgeCastDist = 0.75f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] protected LayerMask lineOfSightLayers;


    [Header("Pathfinding")]
    [SerializeField] private float nextWaypointDistance = 1f;
    private Path path;
    private int currentWaypoint = 0;
    // bool reachedEndOfPath = false;
    Seeker seeker;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 15f;


    [Header("Others")]
    [SerializeField] PlayerDetector closeRangePlayerDetection;
    [SerializeField] PlayerDetector hotzone;
    [SerializeField] PlayerDetector rangeOfSight;

    [Header("Surround")]
    [SerializeField] protected float surroundDistance = 3f;
    [SerializeField] private float surroundSpeed = 2f;
    private float surroundDistance_;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        attackTimer = attackCooldown;
        SwitchToState(initialState);

        playerDistance = Vector2.Distance(transform.position, playerTransform.position);

        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        surroundDistance_ = surroundDistance - 0.15f;

        closeRangePlayerDetection.PlayerEnterCallback += PlayerDetected;
        hotzone.PlayerExitedCallback += StopEngagePlayer;
        rangeOfSight.PlayerStayCallback += SeekPlayer;
    }

    protected void OnDestroy() {
        closeRangePlayerDetection.PlayerEnterCallback -= PlayerDetected;
        hotzone.PlayerExitedCallback -= StopEngagePlayer;
        rangeOfSight.PlayerStayCallback -= SeekPlayer;
    }

    protected virtual void Update() {
        if (currHP <= 0) return;

        if (isAttackCooldown) {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0 && isAttackCooldown) {
                isAttackCooldown = false;
                attackTimer = attackCooldown;
            }
        }

        if (isStunned) return;

        if (playerDetected) {
            playerDistance = Vector2.Distance(transform.position, playerTransform.position);

            DecideState();
        }
    }

    void SeekPlayer() {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.position, lineOfSightLayers);
        Debug.DrawLine(transform.position, playerTransform.position, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player")) {
            playerDetected = true;
        }

        // RaycastHit2D hit = Physics2D.Raycast(lineOfSight.position, transform.right, 
        //     (isFacingRight) ? lineOfSightDistance : -lineOfSightDistance, lineOfSightLayers);
        // Debug.DrawRay(lineOfSight.position, transform.right * ((isFacingRight) ? lineOfSightDistance : -lineOfSightDistance), 
        //     (playerDistance > attackDistance) ? Color.red : Color.green);

        // if (hit.collider != null && hit.collider.CompareTag("Player")) {
        //     playerDetected = true;
        // } 
    }

    protected void FixedUpdate() {
        if (currHP <= 0) return;

        if (!knockbacked) animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        else animator.SetFloat("Speed", 0);

        if (isStunned && !canBeKnockbacked) rb.velocity = new Vector2(0, rb.velocity.y);
        if (isStunned) return;
        
        // state behaviors
        switch (currentState) {
            case State.Idle:
                animator.SetTrigger("Idle");
                break;
            case State.Patrol:
                animator.SetBool("canAttack", false);
                break;
            case State.Chase:
                animator.SetBool("canAttack", false);
                break;
            case State.Surround:
                animator.SetBool("canAttack", false);
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
        // float castDist = (isFacingRight) ? wallCastDist : -wallCastDist;

        // Vector3 targetPos = wallCheck.position;
        // targetPos.x += castDist;

        // Debug.DrawLine(wallCheck.position, targetPos, Color.red);

        // return Physics2D.Linecast(wallCheck.position, targetPos, groundLayer | wallLayer);
        return Physics2D.OverlapCircle(wallCheck.position, wallCastDist, groundLayer | wallLayer);
    }

    bool isNearEdge() {
        Vector3 targetPos = ledgeCheck.position;
        targetPos.y -= ledgeCastDist;

        Debug.DrawLine(ledgeCheck.position, targetPos, Color.red);

        return !Physics2D.Linecast(ledgeCheck.position, targetPos, groundLayer | wallLayer);
    }

    bool isHittingTallWall() {
        float castDist = (isFacingRight) ? ledgeCastDist : -ledgeCastDist;

        Vector3 targetPos = jumpCheck.position;
        targetPos.x += castDist;

        Debug.DrawLine(jumpCheck.position, targetPos, Color.red);

        return Physics2D.Linecast(jumpCheck.position, targetPos, groundLayer | wallLayer);
    }

    bool isBackHittingWall() {
        float castDist = (!isFacingRight) ? 0.25f : -0.25f;

        Vector3 targetPos = backWallCheck.position;
        targetPos.x += castDist;

        Debug.DrawLine(backWallCheck.position, targetPos, Color.blue);

        return Physics2D.Linecast(backWallCheck.position, targetPos, groundLayer | wallLayer);
    }

    bool isBackNearEdge() {
        Vector3 targetPos = backLedgeCheck.position;
        targetPos.y -= ledgeCastDist;

        Debug.DrawLine(backLedgeCheck.position, targetPos, Color.blue);

        return !Physics2D.Linecast(backLedgeCheck.position, targetPos, groundLayer | wallLayer);
    }


    bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    // call this at last frame of attack animation
    public void StartAttackCooldown() {
        isAttackCooldown = true;
        animator.SetBool("canAttack", false);

        if (playerDetected) LookAtPlayer();
    }

    void PlayerDetected() {
        playerDetected = true;
    }

    public void StopEngagePlayer() {
        playerDetected = false;
        SwitchToState(MeleeEnemyController.State.Patrol);
    }

    protected virtual void DecideState() {
        if (0.5 >= playerDistance) { SwitchToState(State.Surround); }
        else if (playerDistance > attackDistance) {
            SwitchToState(State.Chase);
        } else if (attackDistance >= playerDistance && !isAttackCooldown) {
            SwitchToState(State.Attack);
        } else if (attackDistance >= playerDistance && isAttackCooldown) {
            SwitchToState(State.Idle);
        }
    }

    void SetMovement() {
        // state movement behavior
        if (knockbacked) {
            //float lerpedXVelocity = Mathf.Lerp(rb.velocity.x, 0f, Time.fixedDeltaTime);
            //rb.velocity = new Vector2(lerpedXVelocity, rb.velocity.y);
            return;
        }

        switch (currentState) {
            case State.Idle:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case State.Patrol:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    float vX = (isFacingRight) ? moveSpeed : -moveSpeed;

                    rb.velocity = new Vector2(vX, rb.velocity.y);    

                    if (isHittingTallWall() || isNearEdge()) {
                        flipX();
                    } else if (isHittingWall() && IsGrounded()) {
                        rb.velocity = Vector2.up * jumpForce;
                    }
                }
                break;
            case State.Chase:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    ChasePlayerAI();
                }

                break;
            case State.Surround:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    LookAtPlayer();

                    if (surroundDistance_ > playerDistance && !isBackHittingWall() && !isBackNearEdge()) {
                        float vX = (isFacingRight) ? -surroundSpeed : surroundSpeed;
                        rb.velocity = new Vector2(vX, rb.velocity.y);
                    } else {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                break;
            case State.Attack:
                AttackBehavior();
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
                case State.Chase:
                    UpdatePath();
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


    void ChasePlayer() {
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


    void UpdatePath() {
        if (playerDetected && seeker.IsDone())
            seeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
    }

    protected virtual void ChasePlayerAI() {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);

        // if (Mathf.Abs(dir.x) < 0.1) { 
        //     rb.velocity = new Vector2(0, rb.velocity.y);
        //     LookAtPlayer();
        // } else {
        //     if (dir.x > 0.1) rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
        //     else if (dir.x < -0.1) rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
        //     FaceMovementDirection();
        // }

        if (dir.x > 0.1) {
            rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
        }
        else if (dir.x < -0.1) {
            rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
        }

        // jump if needed
        if (isHittingWall() && !isHittingTallWall() && IsGrounded()) {
            rb.velocity = Vector2.up * jumpForce;
        }

        FaceMovementDirection();

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected virtual void AttackBehavior() {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
