using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : StateMachineBehaviour
{
    public float speed = 2f;

    EnemyController enemyController;
    Rigidbody2D rb;
    Transform playerTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyController = animator.GetComponent<EnemyController>();
        rb = animator.GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Vector2 targetPos = new Vector2(playerTransform.position.x, rb.position.y);
        // Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.deltaTime);
        // enemyController.transform.position = newPos;
        
        if (enemyController.PlayerInAttackRange()) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            Debug.Log("player in attack range");
        }
        else if (rb.position.x < playerTransform.position.x) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            Debug.Log("moving right");
        }
        else if (rb.position.x > playerTransform.position.x) {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            Debug.Log("moving left");
        }

        enemyController.LookAtPlayer();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
