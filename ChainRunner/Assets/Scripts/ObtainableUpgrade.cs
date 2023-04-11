using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableUpgrade : MonoBehaviour
{
    public enum UpgradeList
    {
        Zipline, Walljump, DamageIncrease, HealthIncrease
    }
    public UpgradeList upgrade;

    private bool nearObject = false;

    public float damageMultiplier = 1.2f;
    public float healthMultiplier = 1.2f;

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
            case UpgradeList.DamageIncrease:
                break;
            case UpgradeList.HealthIncrease:
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
                    GameManager.g.ShowTitleEffect("Chainhook Obtained");
                    PlayerController.p.SaveOptions();
                    break;
                case UpgradeList.Walljump:
                    PlayerController.p.wallJumpEnabled = true;
                    GameManager.g.ShowTitleEffect("Walljump Obtained");
                    PlayerController.p.SaveOptions();
                    break;
                case UpgradeList.DamageIncrease:
                    PlayerController.p.MultiplyDamage(damageMultiplier);
                    GameManager.g.ShowTitleEffect("Damage Increased!");
                    PlayerController.p.SaveOptions();
                    break;
                case UpgradeList.HealthIncrease:
                    PlayerController.p.MultiplyHealth(healthMultiplier);
                    GameManager.g.ShowTitleEffect("Health Increased!");
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
