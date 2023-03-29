using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftArm : MonoBehaviour
{
    /* ATTACK
     * 
     * Left click to punch if nothing held
     * F to pick up item
     * hold F to drop current item
     * Left click to throw currently held item
     * 
     * If grappling and nothing held, don't punch till grapple done 
     * 
     */

    public GameObject projectilePrefab;
    [SerializeField] private PlayerController p;
    [SerializeField] private Transform shootpoint;
    private bool hasItem = false;
    public Image ItemSpriteHolder;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = p.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        if (!p.LockFlipDirection && Input.GetMouseButtonDown(0))
        {
            if (hasItem)
            {
                ThrowItem();
            }
            else
            {
                Punch();
            }
        }
    }

    public bool GrabItem(Sprite s)
    {
        if (!hasItem)
        {
            ItemSpriteHolder.sprite = s;
            ItemSpriteHolder.gameObject.SetActive(true);
            hasItem = true;
            return true;
        }
        else
        {
            return false;
        } 
    }

    public void ThrowItem()
    {
        hasItem = false;
        ItemSpriteHolder.gameObject.SetActive(false);
        p.ThrowItem();
    }
    public void ActuallyThrowItem()
    {
        Projectile g = Instantiate(projectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
        g.SetSprite(ItemSpriteHolder.sprite);
        g.rb.velocity = g.transform.up * (p.rb.velocity.magnitude + g.speed);
    }

    public void Punch()
    {
        p.Punch();
    }
}
