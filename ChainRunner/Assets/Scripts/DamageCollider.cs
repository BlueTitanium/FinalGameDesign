using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public bool isPlayerOwned = true;
    public float damage = 10f;
    public float kbAmount = 2f;
    public GameObject HitEffect;
    public Transform hitPoint;

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
        if (isPlayerOwned && collision.CompareTag("Enemy"))
        {
            Enemy enemyController = collision.GetComponent<Enemy>();
            enemyController.Knockback(transform, kbAmount);
            enemyController.TakeDamage(damage);
            if(HitEffect != null)
            {
                var g = Instantiate(HitEffect, hitPoint.position, HitEffect.transform.rotation);
                if(PlayerController.p.transform.localScale.x < 0)
                {
                    g.transform.localScale = new Vector3(-g.transform.localScale.x, g.transform.localScale.y, g.transform.localScale.z);
                }
            }
        }
    }
}
