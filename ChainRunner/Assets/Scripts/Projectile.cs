using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerOwned = true;
    public float speed = 10f;
    public float damage = 10f;
    public Rigidbody2D rb;
    public GameObject explosion;
    private Transform child;
    public ThrowableObject.ObjectType type;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        child = transform.GetChild(0);
        transform.DetachChildren();
        Destroy(child.gameObject, 1f);
    }
    public void SetSprite(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isPlayerOwned && collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.PlaySound("itemHit");
            Enemy enemyController = collision.gameObject.GetComponent<Enemy>();
            enemyController.Knockback(transform, 2);
            enemyController.TakeDamage(damage);
            enemyController.Stun(1.5f);
            Destroy(gameObject);
        }

        if (isPlayerOwned && collision.gameObject.CompareTag("Boss"))
        {
            AudioManager.PlaySound("itemHit");
            Cleopatra.c.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (!isPlayerOwned && collision.gameObject.CompareTag("Player"))
        {
            PlayerController.p.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.PlaySound("itemFall");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("HookPoint"))
        {
            AudioManager.PlaySound("itemFall");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isPlayerOwned && collision.gameObject.CompareTag("Enemy")) {
            if (type == ThrowableObject.ObjectType.Molotov) {
                AudioManager.PlaySound("molotov");
            }
            else if (type == ThrowableObject.ObjectType.StraightProjectile) {
                AudioManager.PlaySound("itemHit");
            } else {
                AudioManager.PlaySound("itemFall");
            }
            Enemy enemyController = collision.GetComponent<Enemy>();
            enemyController.Knockback(transform, 2);
            enemyController.TakeDamage(damage);
            enemyController.Stun(1.5f);
            Destroy(gameObject);
        }

        if (isPlayerOwned && collision.gameObject.CompareTag("Boss"))
        {
            AudioManager.PlaySound("itemHit");
            Cleopatra.c.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (!isPlayerOwned && collision.gameObject.CompareTag("Player")) {
            PlayerController.p.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (type == ThrowableObject.ObjectType.Molotov) {
                AudioManager.PlaySound("molotov");
            }
            else if (type == ThrowableObject.ObjectType.StraightProjectile) {
                AudioManager.PlaySound("itemFall");
            } //else {
            //     AudioManager.PlaySound("itemFall");
            // }    
            Destroy(gameObject); 
        }
        

        if (collision.gameObject.CompareTag("HookPoint"))
        {
            if (type == ThrowableObject.ObjectType.Molotov) {
                AudioManager.PlaySound("molotov");
            }
            else if (type == ThrowableObject.ObjectType.StraightProjectile) {
                AudioManager.PlaySound("itemFall");
            } //else {
            //     AudioManager.PlaySound("itemFall");
            // }
            Destroy(gameObject);
        }
    }

}
