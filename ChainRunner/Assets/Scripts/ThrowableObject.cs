using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{

    private bool nearObject = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {


        if (nearObject && Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerController.p.arm.GrabItem(GetComponent<SpriteRenderer>().sprite))
            {
                GameManager.g.ItemPickupDisplayToggle(false);
                Destroy(transform.parent.gameObject);
            }
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
