using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI d;

    public Animation anim;

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
    public bool[] isPlayer;
    private int index;
    public float wordSpeed;

    public bool dialogueActive = false;

    private char[] characters;
    private int cIndex = 0;

    private bool playerRightOfNPC;
    private void Start()
    {
        d = this;
    }


    public void Continue()
    {
        if (dialogueText.text == dialogue[index])
        {
            NextLine();
        }
        else
        {
            dialogueText.text = dialogue[index];
            cIndex = characters.Length;
        }
    }

    public void DialogueActivate(string name, string[] dialogues, bool[] playerD, Sprite icon, bool faceLeft)
    {

        if (dialogueActive)
        {
            if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
            else
            {
                dialogueText.text = dialogue[index];
                cIndex = characters.Length;
            }
        } else
        {
            cName = name;
            dialogue = dialogues;
            isPlayer = playerD;
            npcIcon = icon;
            rightCharRend.enabled = true;
            rightCharRend.sprite = npcIcon;
            rightOverlay.sprite = npcIcon;
            if (faceLeft)
            {
                rightCharScale.localScale = new Vector3(-2, 2);
            } 
            else
            {
                rightCharScale.localScale = new Vector3(2, 2);
            }
            StartCoroutine(StartDialogue());
            
        }
    }
    public IEnumerator StartDialogue()
    {
        dialogueActive = true;
        dialogueText.text = "";
        index = 0;
        if (isPlayer.Length > index && isPlayer[index])
        {
            charName.text = "Player";
            leftOverlay.enabled = false;
            rightOverlay.enabled = true;
        }
        else 
        {
            charName.text = cName;
            leftOverlay.enabled = true;
            rightOverlay.enabled = false;
        }
        anim.Play("Dialogue_Start");
        yield return new WaitForSeconds(anim.GetClip("Dialogue_Start").length);
        characters = dialogue[index].ToCharArray();
        cIndex = 0;
        StartCoroutine(Typing());
    }
    public void EndDialogue()
    {
        if (dialogueActive)
        {
            dialogueActive = false;
            index = 0;
            StartCoroutine(TryEndDialogue());
        }
    }
    IEnumerator TryEndDialogue()
    {
        yield return new WaitUntil(()=>!anim.isPlaying);
        anim.Play("Dialogue_End");
    }
    public void ShowHUD()
    {
        GameManager.g.BringBackHud();
    }
    public void HideHUD()
    {
        GameManager.g.RemoveHud();
    }
    public void NextLine()
    {

        //contArrow.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;

            dialogueText.text = "";
            characters = dialogue[index].ToCharArray();
            cIndex = 0;
            StartCoroutine(Typing());
        }
        else
        {
            EndDialogue();
        }
    }

    public void noText()
    {
        dialogueActive = false;
        dialogueText.text = "";
        index = 0;
    }

    IEnumerator Typing()
    {
        if (isPlayer.Length > index  && isPlayer[index])
        {
            charName.text = "Player";
            leftOverlay.enabled = false;
            rightOverlay.enabled = true;
        }
        else
        {
            charName.text = cName;
            leftOverlay.enabled = true;
            rightOverlay.enabled = false;
        }
        while (cIndex < characters.Length && dialogueText.text.Length < characters.Length)
        {
            dialogueText.text += characters[cIndex];
            cIndex += 1;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            contArrow.SetActive(true);
        }
    }
}
