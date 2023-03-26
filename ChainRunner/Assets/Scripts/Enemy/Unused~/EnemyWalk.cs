using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : StateMachineBehaviour
{
    public float speed = 1f;

    private float timer;
    public float minTime;
    public float maxTime;

    EnemyController enemyController;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyController = animator.GetComponent<EnemyController>();
        rb = animator.GetComponent<Rigidbody2D>();

        timer = Random.Range(minTime, maxTime);
        if (Random.value > 0.5f) enemyController.flipX(); // choose new direction to patrol
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float vX = (enemyController.isFacingRight) ? speed : -speed;
        rb.velocity = new Vector2(vX, rb.velocity.y);    

        if (enemyController.isHittingWall() ||enemyController.isNearEdge()) {
            enemyController.flipX();
        }

        if (timer <= 0) {
            animator.SetTrigger("Idle");
        } else {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
