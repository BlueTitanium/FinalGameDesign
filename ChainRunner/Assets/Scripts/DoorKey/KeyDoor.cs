using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour {

    [SerializeField] private int keyType;
    private bool nearObject;
    private Animator a;
    public bool opened = false;

    private void Start()
    {
        a = GetComponent<Animator>();
    }

    void Update()
    {
        if (nearObject && !opened && KeyHolder.k.ContainsKey(keyType) && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }



    public void OpenDoor() {
        opened = true;
        a.SetTrigger("Open");
        GameManager.g.InteractionDisplayToggle(false);
    }

    public void CloseDoor()
    {
        opened = false;
        a.SetTrigger("Close");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            if (!opened && KeyHolder.k.ContainsKey(keyType))
            {
                GameManager.g.InteractionDisplayToggle(nearObject);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = false;
            GameManager.g.InteractionDisplayToggle(false);
        }
    }
}
