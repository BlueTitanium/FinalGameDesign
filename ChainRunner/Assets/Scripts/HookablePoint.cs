using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookablePoint : MonoBehaviour
{
    LayerMask l;
    // Start is called before the first frame update
    void Start()
    {
        l = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hello");
            gameObject.layer = 0;
        }
    }

   
   
    IEnumerator TempDisable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(.1f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.layer = l;
        }
    }

}
