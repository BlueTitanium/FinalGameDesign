using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int id = 1;
    public Animator a;
    private bool nearObject = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nearObject && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController.p.TakeHeal(PlayerController.p.maxHP);
            CheckpointController.c.curCheckPointID = id;
            CheckpointController.c.SaveOptions();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            GameManager.g.InteractionDisplayToggle(nearObject);
            a.SetTrigger("Open");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = false;
            GameManager.g.InteractionDisplayToggle(nearObject);
            a.SetTrigger("Close");
        }
    }
}
