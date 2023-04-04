using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public Sprite npcIcon;

    public string cName;
    public string[] dialogue;
    public bool[] isPlayer;
    public bool faceLeft;
    public bool playerIsNear;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear) {
            DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            GameManager.g.InteractionDisplayToggle(playerIsNear);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsNear = false;
            GameManager.g.InteractionDisplayToggle(playerIsNear);
            DialogueUI.d.EndDialogue();
        }
    }

        
}
