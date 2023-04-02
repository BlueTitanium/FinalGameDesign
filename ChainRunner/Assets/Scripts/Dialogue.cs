using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject player;
    public GameObject dialoguePanel;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI dialogueText;
    public GameObject contArrow;


    public Image leftCharRend;
    public Image rightCharRend;
    public RectTransform rightCharScale;
    public Image leftOverlay;
    public Image rightOverlay;
    public Sprite npcIcon;

    public string cName;
    public string[] dialogue;

    private int index;
    public float wordSpeed;
    public bool playerIsNear;
    //public GameObject contButton;

    private bool playerRightOfNPC;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear) {

            rightCharRend.sprite = npcIcon;

            if (this.gameObject.name == "aeneas" || this.gameObject.name == "hector" || this.gameObject.name == "homer" || this.gameObject.name == "plato") {
                rightCharScale.localScale = new Vector3(-2, 2); // npc face left
            } else {
                rightCharScale.localScale = new Vector3(2, 2); // npc face left
            }

            if (player.transform.position.x < this.gameObject.transform.position.x) {
                playerRightOfNPC = false;
            } else { 
                playerRightOfNPC = true;

            }


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
        if (this.gameObject.name == "aeneas") {
            if (index == 2 || index == 3) {
                charName.text = "Player";
                leftOverlay.enabled = false;
                rightOverlay.enabled = true;
            } else {
                charName.text = cName;
                leftOverlay.enabled = true;
                rightOverlay.enabled = false;
            }
        } else {
            charName.text = cName;
            leftOverlay.enabled = true;
            rightOverlay.enabled = false;
        }
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

        
}
