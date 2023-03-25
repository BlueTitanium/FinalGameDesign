using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
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

    public bool GrabItem()
    {
        if (!hasItem)
        {
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
        Projectile g = Instantiate(projectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
        g.rb.velocity = g.transform.up * (p.rb.velocity.magnitude + g.speed);
        hasItem = false;
    }

    public void Punch()
    {

    }
}
