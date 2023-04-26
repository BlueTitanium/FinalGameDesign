using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleopatra : MonoBehaviour
{

    public static Cleopatra c;


    /// <summary>
    /// states/attacks: 
    /// 
    /// - prebattle
    ///     - dialogue
    /// 
    /// - idle
    ///     - move to standard pos
    /// 
    /// - floating
    ///     - move around freely away from player
    /// 
    /// - shoot
    ///     - rotate towards target
    ///     
    /// - tower
    ///     - spawn cat tower on player location
    ///     
    /// - spawn towers on walls
    /// 
    /// - spawn towers in a line towards player
    /// 
    /// -
    /// 
    /// -
    /// </summary>

    public enum CleopatraStates
    {
        prebattle,
        idle,
        floating,
        attacking,
    }
    public CleopatraStates state = CleopatraStates.prebattle;


    private Rigidbody2D rb;

    public Transform target;

    public bool isDead = false;
    
    public Transform rightwall;
    public Transform leftwall;
    public Transform center;

    public float distToPlayer;
    public float range = 4f;
    public float speed;
    private float ogSpeed;

    public float cdTimeLeft = 0f;
    public float cdTime = .6f;


    private Animator a;

    [Header("HP Bar")]
    public float maxHP;
    public float curHP;
    [SerializeField]
    private Animation HPbar;
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;
    [SerializeField]
    private float chipSpeed = 2f;
    private float lerpTimer;


    private bool doneOnce;
    [Header ("Dialogue")]
    public Sprite npcIcon;

    public string cName;
    public string[] dialogue;
    public bool[] isPlayer;
    public bool faceLeft;
    public bool playerIsNear;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        //BOSSUI.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
      
        target = PlayerController.p.transform;
        curHP = maxHP;

        PlayerController.p.allowControls = false;

        //isDead = true;
        //StartCoroutine(BeginFight());
        //ogSpeed = speed;
        a = GetComponent<Animator>();
        //aud = GetComponent<AudioSource>();

    }

    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = curHP / maxHP;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }


    //public IEnumerator BeginFight()
    //{

    //    yield return new WaitForSeconds(0.9f);
    //    BOSSUI.SetActive(true);
    //    isDead = false;
    //}
    //public void EndFight()
    //{
    //    isDead = true;
    //    e.hp = 0;

    //    if (drop != null)
    //        Instantiate(drop, transform.position, drop.transform.rotation);
    //    rb.gravityScale = 1f;

    //    StartCoroutine(Die(1));
    //}

    //public IEnumerator Die(float time)
    //{
    //    FindObjectOfType<CameraShaker>().ShakeCamera(6, .5f);
    //    yield return new WaitForSecondsRealtime(0.1f);
    //    if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name != "LofiGirl_Death")
    //    {
    //        a.SetTrigger("Death");
    //    }
    //    FindObjectOfType<CameraShaker>().ShakeCamera(6, .1f);
    //    yield return new WaitForSecondsRealtime(time);
    //    BOSSUI.SetActive(false);
    //    Destroy(parent.gameObject);
    //}

    public void TryAttack()
    {
        //if (cdTimeLeft <= 0)
        //{
        //    int curAttack = Random.Range(0, maxNum);
        //    switch (curAttack)
        //    {
        //        case 0:
        //            a.SetTrigger("Slash");
        //            break;
        //        case 1:
        //            a.SetTrigger("Thrust");
        //            break;
        //        default:
        //            break;
        //    }
        //    cdTimeLeft = cdTime;
        //}
    }

    //public void TryShield()
    //{
    //    if (cdTimeLeft <= 0)
    //    {
    //        a.SetTrigger("ShieldOn");
    //        aud.PlayOneShot(whoosh);
    //        cdTimeLeft = .4f;
    //    }
    //}

    //public void TryShoot()
    //{
    //    //if (!isDead)
    //    //{
    //    //    if (cdTimeLeft <= 0)
    //    //    {

    //    //        int curAttack = Random.Range(0, 4);
    //    //        print(curAttack);
    //    //        if (e.hp / e.maxHP < 0.5)
    //    //        {
    //    //            print("second half");
    //    //            switch (curAttack)
    //    //            {
    //    //                case 0:
    //    //                    a.SetTrigger("Shoot1");
    //    //                    aud.PlayOneShot(shoot1);
    //    //                    break;
    //    //                case 1:
    //    //                    a.SetTrigger("SummonUp");
    //    //                    aud.PlayOneShot(shoot2);
    //    //                    break;
    //    //                case 2:
    //    //                    a.SetTrigger("Shoot1");
    //    //                    aud.PlayOneShot(shoot1);
    //    //                    break;
    //    //                case 3:
    //    //                    a.SetTrigger("Shoot1");
    //    //                    aud.PlayOneShot(shoot1);
    //    //                    break;
    //    //                default:
    //    //                    break;
    //    //            }
    //    //        }
    //    //        else
    //    //        {

    //    //            print("first half");
    //    //            if (summonObject.activeInHierarchy == false)
    //    //            {
    //    //                a.SetTrigger("SummonUp");
    //    //                aud.PlayOneShot(shoot2);
    //    //            }
    //    //        }
    //    //        cdTimeLeft = cdTime;
    //    //    }
    //    //}
    //}

    //public IEnumerator SummonUp()
    //{
    //    if (!isDead)
    //    {
    //        summonObject.SetActive(true);
    //        yield return new WaitForSeconds(1);
    //        summonObject.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CleopatraStates.prebattle:
                if (!doneOnce)
                {
                    DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
                    doneOnce = true;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                    {
                        DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
                    }
                    if (doneOnce && !DialogueUI.d.dialogueActive)
                    {
                        state = CleopatraStates.idle;
                        PlayerController.p.allowControls = true;
                        HPbar.Play();
                    }
                }
                
                break;
            case CleopatraStates.idle:
                if(target.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                } else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                break;
            case CleopatraStates.floating:
                break;
            case CleopatraStates.attacking:
                break;
            default:
                break;
        }
        //    if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LofiGirl_Shoot" || a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LofiGirl_SummonShot" || summonObject.activeInHierarchy)
        //    {
        //        isShooting = true;
        //    }
        //    else
        //    {
        //        isShooting = false;
        //    }
        //    if (cdTimeLeft > 0)
        //    {
        //        cdTimeLeft -= Time.deltaTime;
        //    }

        //    bossHPUI.fillAmount = e.hp / e.maxHP;
        //    if (e.hp / e.maxHP < 0.5)
        //    {
        //        speed = ogSpeed * 1.25f;
        //    }
        //    if (!isDead)
        //    {
        //        distToPlayer = Vector2.Distance(parent.position, target.position);
        //        Vector2 dir = (parent.position - target.position);
        //        Vector2 dir2 = (parent.position - origin);
        //        if (!isShooting)
        //        {
        //            if (-dir.x > 0)
        //            {
        //                parent.localScale = new Vector3(-1, parent.localScale.y, 1);
        //            }
        //            else
        //            {
        //                parent.localScale = new Vector3(1, parent.localScale.y, 1);
        //            }
        //        }

        //        if (distToPlayer <= range)
        //        {
        //            rb.velocity = Vector2.zero;
        //        }
        //        else
        //        {
        //            if (e.hp / e.maxHP >= 0.5) rb.velocity = new Vector2(-dir2.normalized.x * speed, 0f);
        //            else rb.velocity = new Vector2(-dir.normalized.x * speed, 0f);
        //            //rb.velocity = -dir.normalized * speed;
        //        }
        //        if (e.hp / e.maxHP >= 0.5 && distToPlayer <= range)
        //        {
        //            TryShield();
        //        }
        //        else if (e.hp / e.maxHP < 0.5 && distToPlayer <= 4f)
        //        {
        //            TryShield();
        //        }
        //        else
        //        {
        //            TryShoot();

        //        }
        //    }

    }
}
