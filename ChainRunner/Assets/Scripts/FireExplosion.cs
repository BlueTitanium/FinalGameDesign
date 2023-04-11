using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    public bool isPlayerOwned = true;
    public float damagePerTick = 3f;
    public float tickTime = .2f;
    public float tickTimeLeft = 0;

    public List<Enemy> enemiesIn;
    public bool isPlayerIn = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public float speed = -1;

    private void Start()
    {
        Destroy(gameObject, 6);
    }
    private bool IsGrounded()
    {
        var grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return grounded;
    }
    private void Update()
    {

        if (!IsGrounded())
        {
            transform.Translate(new Vector2(0,speed*Time.deltaTime));
        }

        if(tickTimeLeft > 0)
        {
            tickTimeLeft -= Time.deltaTime;
        }
        if(tickTimeLeft <= 0)
        {
            if (isPlayerOwned)
            {
                foreach(var e in enemiesIn)
                {
                    e.TakeDamage(damagePerTick, false);
                }
            }
            else
            {
                if (isPlayerIn)
                {
                    PlayerController.p.TakeDamage(damagePerTick);
                }
            }
            tickTimeLeft = tickTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerOwned && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (!enemiesIn.Contains(e))
            {
                enemiesIn.Add(e);
            }
        }

        if (!isPlayerOwned && collision.gameObject.CompareTag("Player"))
        {
            isPlayerIn = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayerOwned && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (enemiesIn.Contains(e))
            {
                enemiesIn.Remove(e);
            }
        }

        if (!isPlayerOwned && collision.gameObject.CompareTag("Player"))
        {
            isPlayerIn = false;
        }
    }
}