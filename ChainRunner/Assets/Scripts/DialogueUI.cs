using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
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

    private bool playerRightOfNPC;

    void Update()
    {
        rightCharRend.sprite = npcIcon;

        if (player.transform.position.x < this.gameObject.transform.position.x) {
            playerRightOfNPC = false;
        } else { 
            playerRightOfNPC = true;
        }

    }
}
