using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RiftTempTP : MonoBehaviour
{

    public int id = 1;
    public Animator a;
    private bool nearObject = false;
    public string scene = "JessDemoLimbo";
    private bool doneOnce = false;

    public Sprite npcIcon;

    public string cName;
    public string[] dialogue;
    public bool[] isPlayer;
    public bool faceLeft;
    public bool playerIsNear;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (nearObject && Input.GetKeyDown(KeyCode.E))
        {
            if (!doneOnce)
            {
                DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
                doneOnce = true;
            } else
            {
                DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
            }
        }
        if (doneOnce && !DialogueUI.d.dialogueActive)
        {
            SceneManager.LoadScene(scene);
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
            doneOnce = false;
            DialogueUI.d.EndDialogue();
        }
    }
}
