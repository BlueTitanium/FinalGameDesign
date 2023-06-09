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
        postbattle
    }
    public CleopatraStates state = CleopatraStates.prebattle;

    public Canvas canvas;

    private Rigidbody2D rb;

    public Transform target;

    public bool isDead = false;

    public Transform rightwall;
    public Transform leftwall;
    public Transform center;

    public Transform shootpoint;
    public GameObject shootObject;
    public GameObject catTower;
    public Transform catPoint;
    public GameObject AttackIndicator;
    public Transform[] catSpawns;
    public Transform[] shootPoints;
    public Transform[] shootPoints1;

    public GameObject explosion;

    public float distToPlayer;
    public float range = 4f;
    public float speed;
    private float ogSpeed;

    public float cdTimeLeft = 0f;
    public float cdTime = .6f;

    public float switchTime = 10f;
    public float switchTimeLeft = 0;

    private Animator a;
    public Animation indicator;

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
    [Header("Dialogue")]
    public Sprite npcIcon;

    public string cName;
    public string[] dialogue;
    public bool[] isPlayer;
    public bool faceLeft;
    public bool playerIsNear;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        c = this;
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
        canvas.worldCamera = Camera.main;

    }

    public void TakeDamage(float dmg)
    {
        AudioManager.PlaySound("enemyHurt");
        dmg *= PlayerController.p.damageMultiplier;
        curHP -= dmg;
        DmgTextController.d.SpawnDmgText(dmg, transform.position);
        a.SetTrigger("Damage");
        if (curHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        state = CleopatraStates.postbattle;
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(transform.parent.gameObject);
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
        if (cdTimeLeft <= 0)
        {
            int maxNum = (curHP <= maxHP / 2) ? 3 : 9;
            int curAttack = Random.Range(0, 9);
            switch (curAttack)
            {
                case 0:
                    if(curHP <= maxHP / 2)
                    {
                        a.SetTrigger("Attack1");
                        AudioManager.PlaySound("warning");
                    } else
                    {
                        a.SetTrigger("Attack");
                        AudioManager.PlaySound("warning");
                    }
                    
                    break;
                case 1:
                    StartCoroutine(SpawnLotsOfCats());
                    break;
                default:
                    if (curHP <= maxHP / 2)
                    {
                        a.SetTrigger("Attack1");
                        AudioManager.PlaySound("warning");
                    }
                    else
                    {
                        a.SetTrigger("Attack");
                        AudioManager.PlaySound("warning");
                    }
                    break;
            }
            cdTimeLeft = cdTime;
        }
    }
    public void TryFloatAttack()
    {
        if (cdTimeLeft <= 0)
        {
            int curAttack = Random.Range(0, 2);
            print("flightattack");
            switch (curAttack)
            {
                case 0:
                    StartCoroutine(ShootLots1());
                    break;
                case 1:
                    StartCoroutine(ShootLots2());
                    break;
                default:
                    break;
            }
            cdTimeLeft = cdTime;
        }
    }
    public void TryCloseAttack()
    {
        if (cdTimeLeft <= 0)
        {
            int curAttack = Random.Range(0, 3);
            switch (curAttack)
            {
                case 0:
                    StartCoroutine(SpawnCatClose());
                    break;
                case 1:
                    StartCoroutine(SpawnLotsOfCats());
                    break;
                default:
                    StartCoroutine(SpawnCatClose());
                    break;
            }
            cdTimeLeft = cdTime*2;
        }
    }
    public void TryFlightCloseAttack()
    {
        if (cdTimeLeft <= 0)
        {
            int curAttack = Random.Range(0, 3);
            print("flightattack");
            switch (curAttack)
            {
                case 0:
                    StartCoroutine(SpawnCatClose());
                    break;
                case 1:
                    StartCoroutine(ShootLots2());
                    break;
                case 2:
                    StartCoroutine(SpawnLotsOfCats());
                    break;
                default:
                    StartCoroutine(SpawnCatClose());
                    break;
            }
            cdTimeLeft = cdTime * 2;
        }
    }

    IEnumerator SpawnCatClose()
    {
        indicator.Play();
        AudioManager.PlaySound("warning");
        yield return new WaitForSeconds(.3f);
        GameObject g = Instantiate(catTower, catPoint.position, catPoint.rotation);
        g.transform.localScale = transform.localScale;
        AudioManager.PlaySound("bossAttack");
    }

    IEnumerator SpawnLotsOfCats()
    {
        indicator.Play();
        AudioManager.PlaySound("warning");
        yield return new WaitForSeconds(.3f);
        for (int i =0; i < catSpawns.Length;i++)
        {
            Transform v = catSpawns[i];
            //StartCoroutine(SpawnIndicator(v));
            //yield return new WaitForSeconds(.1f);
            GameObject g = Instantiate(catTower, v.position, v.rotation);
            g.transform.localScale = transform.localScale;
            AudioManager.PlaySound("bossAttack");
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator SpawnIndicator(Transform v)
    {
        AudioManager.PlaySound("warning");
        GameObject ind = Instantiate(AttackIndicator, v.position, AttackIndicator.transform.rotation);
        Destroy(ind, 1f);
        yield return null;
    }


    IEnumerator ShootLots1()
    {
        AudioManager.PlaySound("warning");
        //indicator.Play();
        //yield return new WaitForSeconds(.5f);
        foreach (var v in shootPoints)
        {

            Vector2 shootDir = (PlayerController.p.transform.position - v.position).normalized;
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            GrabbableProjectile g = Instantiate(shootObject, v.position, rotation).GetComponentInChildren<GrabbableProjectile>();
            // g.rb.velocity = shootDir * (rb.velocity.magnitude + g.speed);
            StartCoroutine((SpawnIndicator(v)));
            StartCoroutine(DelayedSend(g, shootDir, .5f));
            yield return new WaitForSeconds(.1f);
        }
        
    }

    IEnumerator DelayedSend(GrabbableProjectile g, Vector2 shootDir, float t)
    {
        yield return new WaitForSeconds(t);
        if(g!=null)
            g.rb.velocity = shootDir * g.speed;
    }

    IEnumerator ShootLots2()
    {
        indicator.Play();
        AudioManager.PlaySound("warning");
        yield return new WaitForSeconds(.5f);
        foreach (var v in shootPoints1)
        {

            Vector2 shootDir = (PlayerController.p.transform.position - v.position).normalized;
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GrabbableProjectile g = Instantiate(shootObject, v.position, rotation).GetComponentInChildren<GrabbableProjectile>();
            // g.rb.velocity = shootDir * (rb.velocity.magnitude + g.speed);
            StartCoroutine(DelayedSend(g, shootDir, .5f));
            yield return new WaitForSeconds(.2f);
        }
    }

    


    public void Shoot()
    {
        Vector2 shootDir = (PlayerController.p.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GrabbableProjectile g = Instantiate(shootObject, shootpoint.position, rotation).GetComponentInChildren<GrabbableProjectile>();
        // g.rb.velocity = shootDir * (rb.velocity.magnitude + g.speed);
        g.rb.velocity = shootDir * g.speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
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
                    if (Input.GetKeyDown(KeyCode.E) /*|| Input.GetMouseButtonDown(0)*/)
                    {
                        DialogueUI.d.DialogueActivate(cName, dialogue, isPlayer, npcIcon, faceLeft);
                    }
                    if (doneOnce && !DialogueUI.d.dialogueActive)
                    {
                        state = CleopatraStates.idle;
                        PlayerController.p.allowControls = true;
                        HPbar.Play();
                        cdTimeLeft = 2;
                        switchTimeLeft = switchTime;
                    }
                }
                
                break;
            case CleopatraStates.idle:
                distToPlayer = Vector2.Distance(transform.position, target.position);
                Vector2 dir2 = (transform.position - center.position);
                rb.velocity = new Vector2(-dir2.normalized.x * speed, 0f);
                if (cdTimeLeft > 0)
                {
                    cdTimeLeft -= Time.deltaTime;
                }
                a.SetBool("Float", false);
                if(target.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                } else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                if(distToPlayer > range)
                {
                    TryAttack();
                } else
                {
                    TryCloseAttack();
                }
                

                if(switchTimeLeft > 0)
                {
                    switchTimeLeft -= Time.deltaTime;
                } else
                {
                    state = CleopatraStates.floating;
                    switchTimeLeft = switchTime;
                }
                break;
            case CleopatraStates.floating:
                distToPlayer = Vector2.Distance(transform.position, target.position);
                Vector2 dir = (transform.position - target.position);
                rb.velocity = new Vector2(-dir.normalized.x * speed, -dir.normalized.y * speed);
                if (cdTimeLeft > 0)
                {
                    cdTimeLeft -= Time.deltaTime;
                }
                a.SetBool("Float", true);
                if (target.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                if (distToPlayer > range)
                {
                    TryFloatAttack();
                }
                else
                {
                    TryFlightCloseAttack();
                }

                if (switchTimeLeft > 0)
                {
                    switchTimeLeft -= Time.deltaTime;
                }
                else
                {
                    state = CleopatraStates.idle;
                    switchTimeLeft = switchTime;
                }
                break;
            case CleopatraStates.attacking:
                break;
            case CleopatraStates.postbattle:

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
