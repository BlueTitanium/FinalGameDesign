using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public int key;
    private bool nearObject;

    // Update is called once per frame
    void Update()
    {


        if (nearObject && Input.GetKeyDown(KeyCode.E))
        {
            KeyHolder.k.AddKey(key);
            GameManager.g.ShowTitleEffect("KEY OBTAINED");
            GameManager.g.InteractionDisplayToggle(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            GameManager.g.InteractionDisplayToggle(nearObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = false;
            GameManager.g.InteractionDisplayToggle(nearObject);
        }
    }

}