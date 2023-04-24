using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gem;
    private bool nearObject;

    // Update is called once per frame
    void Update()
    {
        if (GemManager.g.gemStates[gem] != 0)
        {
            Destroy(gameObject);
        }

        if (nearObject && Input.GetKeyDown(KeyCode.E))
        {
            GemManager.g.gemStates[gem] = 1;
            GemManager.g.SaveOptions();
            GameManager.g.ShowTitleEffect("GEM OBTAINED");
            GameManager.g.InteractionDisplayToggle(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = true;
            GameManager.g.InteractionDisplayToggle(nearObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nearObject = false;
            GameManager.g.InteractionDisplayToggle(nearObject);
        }
    }
}
