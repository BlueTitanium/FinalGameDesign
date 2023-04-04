using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableUpgrade : MonoBehaviour
{
    public enum UpgradeList
    {
        Zipline, Walljump, 
    }

    public UpgradeList upgrade;

    private bool nearObject = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        switch (upgrade)
        {
            case UpgradeList.Zipline:
                if (PlayerController.p.grappleEnabled)
                {
                    Destroy(gameObject);
                }
                break;
            case UpgradeList.Walljump:
                if (PlayerController.p.wallJumpEnabled)
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nearObject && Input.GetKeyDown(KeyCode.E))
        {
            switch (upgrade)
            {
                case UpgradeList.Zipline:
                    PlayerController.p.hook.gameObject.SetActive(true);
                    PlayerController.p.chainHookIcon.SetActive(true);
                    PlayerController.p.grappleEnabled = true;
                    PlayerController.p.SaveOptions();
                    break;
                case UpgradeList.Walljump:
                    PlayerController.p.wallJumpEnabled = true;
                    PlayerController.p.SaveOptions();
                    break;
                default:
                    break;
            }
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
