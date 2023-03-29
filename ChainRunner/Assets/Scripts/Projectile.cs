using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerOwned = true;
    public float speed = 10f;
    public float damage = 10f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
