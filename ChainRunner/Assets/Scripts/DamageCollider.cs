using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public bool isPlayerOwned = true;
    public float damage = 10f;
    public float kbAmount = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (isPlayerOwned && collision.CompareTag("Enemy"))
        {
            print("Hello");
            Enemy enemyController = collision.GetComponent<Enemy>();
            enemyController.Knockback(transform, kbAmount);
            enemyController.TakeDamage(damage);
        }
    }
}
