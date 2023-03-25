using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    //public GameObject npc;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;

    private int index;
    public float wordSpeed;
    public bool playerIsNear;
    public GameObject contButton;

    void Start() {
        if (this.gameObject.name == "aeneas") {
            AeneasText();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear) {
            // if (dialogueBox.activeInHierarchy) {
            //     noText();
            // } else {
            //     dialogueBox.SetActive(true);
            //     StartCoroutine(Typing());
            // }
            if (dialogueBox.activeSelf) {
                NextLine();
            } else {
                dialogueBox.SetActive(true);
                StartCoroutine(Typing());
            }

        }

        if (dialogueText.text == dialogue[index]) {
            contButton.SetActive(true);
        }
    }

    public void noText() {
        dialogueText.text = "";
        index = 0;
        dialogueBox.SetActive(false);
    }

    IEnumerator Typing() {
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine() {

        contButton.SetActive(false);

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
        dialogue[0] = "hey";
        dialogue[1] = "wassup";
    }
}
