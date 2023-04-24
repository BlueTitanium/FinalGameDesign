using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemDoor : MonoBehaviour
{
    private bool nearObject;
    public bool readyToOpen = false;
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nearObject && Input.GetKeyDown(KeyCode.E))
        {
            if (!readyToOpen)
            {
                bool allSlotsFilled = true;
                //check if available 
                for (int i = 0; i < 3; i++)
                {

                    if (GemManager.g.gemStates[i] == 0)
                    {
                        allSlotsFilled = false;
                    }
                    if (GemManager.g.gemStates[i] == 1)
                    {
                        
                        GemManager.g.gemStates[i] = 2;
                        GemManager.g.SaveOptions();
                    }
                }
                if (!allSlotsFilled)
                {
                    GameManager.g.ShowTitleEffect("Need More Gems");
                }
                else
                {
                    GameManager.g.ShowTitleEffect("Door Ready To Open");
                    readyToOpen = true;
                }
            }
            else
            {
                OpenDoor();
                nearObject = false;
                GameManager.g.InteractionDisplayToggle(false);
                PlayerController.p.allowControls = false;
            }
        }
    }

    public void OpenDoor()
    {
        anim.Play();
    }
    public void GoNext()
    {
        GameManager.g.LoadNextScene("TaneimTesting");
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            GameManager.g.InteractionDisplayToggle(true);
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
