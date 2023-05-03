using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingMeleeEnemyController : Enemy
{
    [Header("Melee Movement")]
    [SerializeField] private float chaseSpeed = 500f;

    [Header("Melee Attack")]
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    float attackTimer;
    protected bool isAttackCooldown = false;
    protected float playerDistance; // distance between self and player

    [Header("Melee States")]
    [SerializeField] protected State initialState = State.Idle;
    protected State currentState;
    [HideInInspector] public enum State {Idle, Patrol, Chase, Attack, Surround, Return}

    

    [Header("Melee Colliders")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform backWallCheck;
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


    [Header("Others")]
    [SerializeField] PlayerDetector closeRangePlayerDetection;
    
    [SerializeField] PlayerDetector view;
    [SerializeField] PlayerDetector hotzone;

    [Header("Surround")]
    [SerializeField] protected float surroundDistance = 3f;
    [SerializeField] private float surroundSpeed = 200f;
    private float surroundDistance_;

    [Header("Return to SpawnPoint")]
    [SerializeField] bool returnToSpawnpoint;
    [SerializeField] float returnToSpawnpointSpeed = 200f;
    [SerializeField] Transform spawnpoint;
    [SerializeField] float playerYOffset = 1f;

    [Header("Extra Stun Option")]
    [SerializeField] bool fallOnStun;
    float originalLinearDrag;

    [Header("Sound")]
    [SerializeField] protected string attackSound;


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
        view.PlayerStayCallback += SeekPlayer;

        originalLinearDrag = rb.drag;
    }

    
    protected void OnDestroy() {
        closeRangePlayerDetection.PlayerEnterCallback -= PlayerDetected;
        hotzone.PlayerExitedCallback -= StopEngagePlayer;
        view.PlayerExitedCallback += SeekPlayer;
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
        
        if (hit.collider != null && hit.collider.CompareTag("Player")) {
            playerDetected = true;
        }
    }

    protected void FixedUpdate() {
        if (currHP <= 0) return;

        if (!knockbacked) animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        else animator.SetFloat("Speed", 0);

        if (isStunned) { 
            if (fallOnStun) {
                rb.gravityScale = 5;
                rb.drag = 0.25f;
            }
            return; 
        } else {
            if (fallOnStun) {
                rb.gravityScale = 0;
                rb.drag = originalLinearDrag;
            }
        }
        
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
            case State.Return:
                animator.SetBool("canAttack", false);
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

    bool isBackHittingWall() {
        float castDist = (!isFacingRight) ? 0.25f : -0.25f;

        Vector3 targetPos = backWallCheck.position;
        targetPos.x += castDist;

        Debug.DrawLine(backWallCheck.position, targetPos, Color.blue);

        return Physics2D.Linecast(backWallCheck.position, targetPos, groundLayer | wallLayer);
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
        if (!returnToSpawnpoint) SwitchToState(FlyingMeleeEnemyController.State.Patrol);
        else SwitchToState(FlyingMeleeEnemyController.State.Return);
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

                    if (isHittingWall()) {
                        flipX();
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

                    if (surroundDistance_ > playerDistance && !isBackHittingWall()) {
                        // float vX = (isFacingRight) ? -surroundSpeed : surroundSpeed;
                        // rb.velocity = new Vector2(vX, rb.velocity.y);

                        Vector2 dir = (rb.position - new Vector2 (playerTransform.position.x, playerTransform.position.y + playerYOffset)).normalized;
                        if (dir.y <= 0) dir = new Vector2(dir.x, -dir.y);

                        Vector2 force = dir * surroundSpeed * Time.deltaTime;
                        rb.AddForce(force);
                    } else {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                break;
            case State.Attack:
                AttackBehavior();
                break;
            case State.Return:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    ReturnToSpawnpointAI();
                }
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


    void UpdatePath() {
        if (playerDetected && seeker.IsDone()) {
            Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y + playerYOffset);
            seeker.StartPath(rb.position, playerPos, OnPathComplete);
        }
        else if (!playerDetected && currentState == State.Return && seeker.IsDone())
            seeker.StartPath(rb.position, spawnpoint.position, OnPathComplete);
    }

    protected virtual void ChasePlayerAI() {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        
        Vector2 force = dir * chaseSpeed * Time.deltaTime;
        rb.AddForce(force);
        FaceMovementDirection();

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

    }

    protected virtual void ReturnToSpawnpointAI() {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) {
            SwitchToState(State.Patrol);
            return;
        } 

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        
        Vector2 force = dir * returnToSpawnpointSpeed * Time.deltaTime;
        rb.AddForce(force);
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

    public void PlayAttackSFX() {
        if (attackSound != "")
            AudioManager.PlaySound(attackSound);
    }
}
