using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRangedEnemyController : FlyingMeleeEnemyController
{
    [Header("Ranged Attacks")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootpoint;
    [SerializeField] bool retreatDuringAttackCooldown = true;
    bool aimingPlayer = false;
    Vector2 shootDir;

    // Note that surround distance should be >= attack distance
    
    public void SetRangedAttackDir() {
        shootDir = (playerTransform.position - transform.position).normalized;
    }
    public void AttackRange() {
        Projectile g = Instantiate(projectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
        g.rb.velocity = shootDir * g.speed;
    }

    protected override void DecideState() {
        if (!retreatDuringAttackCooldown) {
            base.DecideState();
            return;
        }
        
        RaycastHit2D hit = Physics2D.Linecast(shootpoint.position, playerTransform.position, lineOfSightLayers);
        bool canHitPlayer = hit.collider != null && hit.collider.CompareTag("Player");

        if (playerDistance > surroundDistance) {
            SwitchToState(State.Chase);
        } else if (0.5 >= playerDistance) {
            SwitchToState(State.Surround);
        } else if (surroundDistance >= playerDistance && isAttackCooldown) {
            SwitchToState(State.Surround);
        } else if (attackDistance >= playerDistance && !isAttackCooldown) {
            if (canHitPlayer) SwitchToState(State.Attack);
            else SwitchToState(State.Chase);
        } else if (surroundDistance >= playerDistance && !isAttackCooldown) {
            SwitchToState(State.Chase);
        }
    }

    public void StartAimingPlayer() {
        aimingPlayer = true;
    }

    public void StopAimingPlayer() {
        aimingPlayer = false;
    }


    protected override void AttackBehavior() {
        base.AttackBehavior();
        if (aimingPlayer) LookAtPlayer();
    }
}
