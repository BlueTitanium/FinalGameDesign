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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isPlayerOwned && collision.gameObject.CompareTag("Enemy")) {
            Enemy enemyController = collision.GetComponent<Enemy>();
            enemyController.Knockback(transform, 2);
            enemyController.TakeDamage(damage);
            enemyController.Stun(5f);
            Destroy(gameObject);
        }

        if (!isPlayerOwned && collision.gameObject.CompareTag("Player")) {
            PlayerController.p.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
