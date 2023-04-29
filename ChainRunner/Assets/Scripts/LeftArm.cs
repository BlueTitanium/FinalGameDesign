using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftArm : MonoBehaviour
{
    /* ATTACK
     * 
     * Left click to punch if nothing held
     * F to pick up item
     * hold F to drop current item
     * Left click to throw currently held item
     * 
     * If grappling and nothing held, don't punch till grapple done 
     * 
     */

    public GameObject projectilePrefab;
    public GameObject gravityAffectedProjectilePrefab;
    public GameObject MolotovPrefab;
    [SerializeField] private PlayerController p;
    [SerializeField] private Transform shootpoint;
    public bool hasItem = false;
    public Image ItemSpriteHolder;
    public bool throwing = false;
    public ThrowableObject.ObjectType itemType;
    public float damage = 0;
    public float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.p.allowControls && !DialogueUI.d.dialogueActive && Time.timeScale != 0)
        {

            transform.position = p.transform.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
            if ((!p.LockFlipDirection || (p.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "nPlayer_ChainToss")) && !throwing && Input.GetMouseButtonDown(0))
            {
                if (hasItem)
                {
                    ThrowItem();
                }
                else
                {
                    Punch();
                }
            }
        }
    }

    public bool GrabItem(Sprite s, ThrowableObject.ObjectType type, float damageAmt, float speedAmt)
    {
        if (!hasItem)
        {
            ItemSpriteHolder.sprite = s;
            ItemSpriteHolder.SetNativeSize();
            ItemSpriteHolder.gameObject.SetActive(true);
            itemType = type;
            hasItem = true;
            damage = damageAmt;
            speed = speedAmt;
            return true;
        }
        else
        {
            return false;
        } 
    }

    public void ThrowItem()
    {
        throwing = true;
        p.ThrowItem();
    }
    public void ActuallyThrowItem()
    {
        throwing = false;
        hasItem = false;
        ItemSpriteHolder.gameObject.SetActive(false);
        switch (itemType)
        {
            case ThrowableObject.ObjectType.StraightProjectile:
                {
                    Projectile _g = Instantiate(projectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
                    _g.SetSprite(ItemSpriteHolder.sprite);
                    Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 _dir = (_mousePos - _g.transform.position).normalized;
                    _dir.z = 0;
                    _g.rb.velocity = _dir.normalized * (/*p.rb.velocity.magnitude +*/ speed);
                    _g.damage = damage;
                    _g.speed = speed;
                }
                break;
            case ThrowableObject.ObjectType.GravityAffected:
                {
                    Projectile _g = Instantiate(gravityAffectedProjectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
                    _g.SetSprite(ItemSpriteHolder.sprite);
                    Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 _dir = (_mousePos - _g.transform.position).normalized;
                    _dir.z = 0;
                    _g.rb.velocity = _dir.normalized * (/*p.rb.velocity.magnitude +*/ speed);
                    _g.damage = damage;
                    _g.speed = speed;
                }
                break;
            case ThrowableObject.ObjectType.Molotov:
                {
                    Projectile _g = Instantiate(MolotovPrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
                    _g.SetSprite(ItemSpriteHolder.sprite);
                    Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 _dir = (_mousePos - _g.transform.position).normalized;
                    _dir.z = 0;
                    _g.rb.velocity = _dir.normalized * (/*p.rb.velocity.magnitude +*/ speed);
                    _g.damage = damage;
                    _g.speed = speed;
                }
                break;
            default:
                {
                    Projectile _g = Instantiate(projectilePrefab, shootpoint.position, transform.rotation).GetComponent<Projectile>();
                    _g.SetSprite(ItemSpriteHolder.sprite);
                    Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 _dir = (_mousePos - _g.transform.position).normalized;
                    _dir.z = 0;
                    _g.rb.velocity = _dir.normalized * (/*p.rb.velocity.magnitude +*/ speed);
                    _g.damage = damage;
                    _g.speed = speed;
                }
                break;
        }

        
    }

    public void Punch()
    {
        p.Punch();
    }
}
