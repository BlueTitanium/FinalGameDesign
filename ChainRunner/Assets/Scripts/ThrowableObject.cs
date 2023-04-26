using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public enum ObjectType
    {
        StraightProjectile,
        GravityAffected,
        Molotov,
    }
    private bool nearObject = false;
    public ObjectType type;
    public float damage = 10f;
    public float speed = 20f;


    Rigidbody2D rb;
    Transform playerTransform;
    float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        rb.simulated = false;
        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerDistance = Vector2.Distance(transform.position, playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (nearObject && Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerController.p.arm.GrabItem(GetComponent<SpriteRenderer>().sprite, type, damage, speed))
            {
                AudioManager.PlaySound("playerGrabItem");
                GameManager.g.ItemPickupDisplayToggle(false);
                Destroy(transform.parent.gameObject);
            }
        }
    }


    void LateUpdate() {
        if (!rb.simulated) {
            playerDistance = Vector2.Distance(transform.position, playerTransform.position);
            if (playerDistance <= 25) rb.simulated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            if(!PlayerController.p.arm.hasItem)
                GameManager.g.ItemPickupDisplayToggle(nearObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = false;
            GameManager.g.ItemPickupDisplayToggle(nearObject);
        }
    }

}
