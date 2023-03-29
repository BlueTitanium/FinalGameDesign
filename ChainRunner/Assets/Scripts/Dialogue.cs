using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    //public GameObject npc;
    public GameObject player;
    public GameObject dialoguePanel;
    public GameObject contArrow;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI dialogueText;
    public string cName;
    public string[] dialogue;

    private int index;
    public float wordSpeed;
    public bool playerIsNear;
    //public GameObject contButton;

    void Start() {
        if (this.gameObject.name == "aeneas") {
            AeneasText();
        } 
        if (this.gameObject.name == "aristotle") {
            AristotleText();
        }
        if (this.gameObject.name == "hector") {
            HectorText();
        }
        if (this.gameObject.name == "hippocrates") {
            HippocratesText();
        }
        if (this.gameObject.name == "homer") {
            HomerText();
        }
        if (this.gameObject.name == "lavinia") {
            LaviniaText();
        }
        if (this.gameObject.name == "penthesilea") {
            PenthesileaText();
        }
        if (this.gameObject.name == "plato") {
            PlatoText();
        }
    }

    void Update()
    {
        if (player.transform.position.x < this.gameObject.transform.position.x) {
            print("left");
        } else { 
            print("right");
        }

        if (Input.GetKeyDown(KeyCode.E) && playerIsNear) {
            // if (dialogueBox.activeInHierarchy) {
            //     noText();
            // } else {
            //     dialogueBox.SetActive(true);
            //     StartCoroutine(Typing());
            // }
            if (dialoguePanel.activeSelf) {
                NextLine();
            } else {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }

        }

        if (dialogueText.text == dialogue[index]) {
            contArrow.SetActive(true);
        }
    }

    public void noText() {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() {
        charName.text = cName;
        foreach (char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine() {

        contArrow.SetActive(false);

        if (index < dialogue.Length - 1) {
            index++;
            
            dialogueText.text = "";
            StartCoroutine(Typing());
        } else {
            noText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            print("collide");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsNear = false;
            noText();
        }
    }

    private void AeneasText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Aeneas";
        dialogue[1] = "wassup";
    }
    private void AristotleText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Aristotle";
        dialogue[1] = "wee";
    }
    private void HectorText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Hector";
        dialogue[1] = "wee";
    }
    private void HippocratesText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Hippocrates";
        dialogue[1] = "wee";
    }
    private void HomerText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Homer";
        dialogue[1] = "wee";
    }
    private void LaviniaText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Lavinia";
        dialogue[1] = "wee";
    }
    private void PenthesileaText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Penthesilea";
        dialogue[1] = "wee";
    }
    private void PlatoText() {
        dialogue = new string[2];
        dialogue[0] = "I'm Plato";
        dialogue[1] = "wee";
    }
        
}
