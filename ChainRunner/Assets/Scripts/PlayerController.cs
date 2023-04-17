using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController p;

    /*
     Player Movement:
        - Walk
        - Double jump
        - Dash/roll
        - Wallslide/jump
        - fast fall
     */

    [Header("Gameplay Stats")]
    public float maxHP = 100f;
    public float curHP = 100f;
    public bool LockFlipDirection = false;
    public float damageMultiplier = 1;
    public float healthMultiplier = 1;

    [Header("HP Bar")]
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;
    [SerializeField]
    private float chipSpeed = 2f;
    private float lerpTimer;
    [SerializeField]
    private TextMeshProUGUI curHPText, maxHPText;


    [Header("Movement")]
    [SerializeField]
    private float speed = 8f;
    private float horizontal;
    [SerializeField] [Range(0f, 1f)]
    private float dampingStop, dampingTurn, dampingNormal;
    private float knockBackTimeLeft = 0;

    [Header("Jumping")]
    [SerializeField]
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    
    [SerializeField]
    private float coyoteTime = .2f;
    private float coyoteTimeLeft;
    [SerializeField]
    private float extraJumpPower = 10f;
    [SerializeField]
    private int extraJumps = 1;
    private int extraJumpsLeft;
    [SerializeField]
    private float jumpBufferTime = .2f;
    private float jumpBufferLeft;
    private bool grounded = false;
    private bool closeToGround = false;
    private bool walled = false;

    [Header("Falling")]
    [SerializeField]
    private float fastFallSpeed = -20;

    [Header("Dashing")]
    [SerializeField]
    private float dashSpeed = 5f;
    [SerializeField]
    private float additionalVelocity = 5f;
    [SerializeField]
    private float dashTime = .2f;
    [SerializeField]
    private float dashCD = .5f;
    private float dashTimeLeft, dashCDLeft;

    [Header("Wallslide")]
    [SerializeField]
    private bool isWallSliding;
    [SerializeField]
    private float wallSlidingSpeed = 2f;


    [Header("WallJump")]
    public bool wallJumpEnabled = true;
    [SerializeField]
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField]
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField]
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Weapon")]
    public LeftArm arm;

    [Header("Grappling")]
    public bool grappleEnabled = false;
    [SerializeField] private float grappleSpeed = 30f;
    public bool grappling = false;
    private Vector2 grappleEndPoint;
    public ChainHook hook;
    private float shouldLatch = 0f;
    public GameObject chainHookIcon;


    [Header("Assign Value")]
    public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    //knockback
    //private float kbDir; 
    
    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = curHP / maxHP;
        if(fillB > hFraction)
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

    public void TakeDamage(float amount)
    {
        DmgTextController.d.SpawnDmgText(amount, transform.position);
        CameraShake.cs.cameraShake(.3f, 1.6f);
        curHP -= amount;
        lerpTimer = 0f;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curHPText.text = ""+ (int) curHP;
        anim.SetTrigger("Damaged");
        if(curHP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MultiplyDamage(float amount)
    {
        damageMultiplier += amount;
        PlayerPrefs.SetFloat("DamageMultiplier", damageMultiplier);
    }

    public void MultiplyHealth(float amount)
    {
        healthMultiplier += amount;
        maxHP = 100f * healthMultiplier;
        PlayerPrefs.SetFloat("HealthMultiplier", healthMultiplier);
        TakeHeal(maxHP);
    }

    public void TakeHeal(float amount)
    {
        curHP += amount;
        lerpTimer = 0f;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curHPText.text = "" + (int) curHP;
        maxHPText.text = ""+ (int) maxHP;
    }

    public void GrappleToLocation(Vector2 dir, Vector2 point)
    {
        extraJumpsLeft = extraJumps;
        grappling = true;
        grappleEndPoint = point;
    }

    public void Punch()
    {
        anim.SetTrigger("Punch");
        StartCoroutine(HandleAnimDirection("Player_Punch"));
    }

    IEnumerator HandleAnimDirection(string str)
    {
        
        yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == str);

        LockFlipDirection = true;
        var mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mouse.x < playerScreenPoint.x)
        {
            isFacingRight = false;
            Vector3 localScale = transform.localScale;
            localScale.x = -1f*Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else
        {
            isFacingRight = true;
            Vector3 localScale = transform.localScale;
            localScale.x = 1f * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != str);

        LockFlipDirection = false;
        if(str == "Player_ItemToss")
        {
            arm.throwing = false;
        }
        yield return null;
    }

    public void ThrowItem()
    {
        anim.SetTrigger("ItemToss");
        StartCoroutine(HandleAnimDirection("Player_ItemToss"));
    }
    public void ActuallyThrowItem()
    {
        arm.ActuallyThrowItem();
    }
    public void StartHook()
    {
        anim.SetTrigger("ChainToss");
        StartCoroutine(HandleAnimDirection("Player_ChainToss"));
    }

    private void Start()
    {
        p = this;
        curHP = maxHP;
        LoadOptions();

    }
    public void SaveOptions(bool start = false)
    {
        PlayerPrefs.SetInt("grappleEnabled", grappleEnabled ? 1 : 0);
        PlayerPrefs.SetInt("wallJumpEnabled", wallJumpEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("DamageMultiplier", damageMultiplier);
        print(damageMultiplier);
        PlayerPrefs.SetFloat("HealthMultiplier", healthMultiplier);
        TakeHeal(maxHP);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("grappleEnabled"))
        {
            SaveOptions(true);
        }
        grappleEnabled = PlayerPrefs.GetInt("grappleEnabled") == 1;
        hook.gameObject.SetActive(grappleEnabled);
        chainHookIcon.SetActive(grappleEnabled);
        wallJumpEnabled = PlayerPrefs.GetInt("wallJumpEnabled") == 1;
        healthMultiplier = PlayerPrefs.GetFloat("HealthMultiplier");
        maxHP = 100 * PlayerPrefs.GetFloat("HealthMultiplier");
        damageMultiplier = PlayerPrefs.GetFloat("DamageMultiplier");
        TakeHeal(maxHP);
    }
    private void Update()
    {
        UpdateHealthUI();

        //TESTING BINDS BELOW
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeHeal(10);
        }

            if (grappling)
        {
            shouldLatch = 1f;
            if (!hook.hookSent)
            {
                grappling = false;
            }
            rb.velocity = -(transform.position-hook.hookPoint.position).normalized * grappleSpeed;
            if(Vector2.Distance(transform.position, grappleEndPoint) < 2f)
            {
                hook.TryRetractHook();
                grappling = false;
            }
        }
        if (shouldLatch > 0f)
        {
            shouldLatch -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        IsGrounded(); IsWalled();
        float horizontalV = rb.velocity.x;
        if (dashTimeLeft <= 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            
            horizontalV += horizontal * speed * Time.deltaTime;
            if (Mathf.Abs(horizontal) < .1f)
            {
                horizontalV *= grounded ? 0 : Mathf.Pow((1f - dampingStop), Time.deltaTime * 10f);
            }
            else if (Mathf.Sign(horizontal) != Mathf.Sign(horizontalV))
            {
                horizontalV *= Mathf.Pow(1f - dampingTurn, Time.deltaTime * 10f);
            }
            else
            {
                horizontalV *= Mathf.Pow(1 - dampingNormal, Time.deltaTime * 10f);
            }
        }
        else
        {
            horizontalV = (Mathf.Abs(horizontalV) > Mathf.Abs(horizontal * dashSpeed)) ? Mathf.Sign(horizontal) * Mathf.Abs(horizontalV) + horizontal * additionalVelocity * Time.deltaTime : horizontal * dashSpeed;
        }
        
        //kbDir = horizontal==0? kbDir:horizontal;
        //kbDir = horizontal;

        if(isWallSliding)
            extraJumpsLeft = extraJumps;
        if (grounded)
        {
            coyoteTimeLeft = coyoteTime;
            extraJumpsLeft = extraJumps;
        } 
        else
        {
            coyoteTimeLeft -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferLeft = jumpBufferTime;
        }
        else
        {
            jumpBufferLeft -= Time.deltaTime;
        }
        if (jumpBufferLeft > 0 && wallJumpingCounter <= 0 && !isWallJumping)
        {

            if(coyoteTimeLeft > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                if (grappling)
                {
                    hook.TryRetractHook();
                    grappling = false;
                }
            } else
            {
                if(extraJumpsLeft > 0)
                {
                    extraJumpsLeft -= 1;
                    rb.velocity = new Vector2(rb.velocity.x, extraJumpPower);
                }
                if (grappling)
                {
                    hook.TryRetractHook();
                    grappling = false;
                }
            }        
        }


        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeLeft = 0;
            jumpBufferLeft = 0;
        }


        WallSlide();
        WallJump();
        
        if (!isWallJumping && !LockFlipDirection)
        {
            Flip();
        }
        
        /*
        if (dashCDLeft <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(horizontal == 0)
            {
                horizontal = isFacingRight ? 1 : -1;
            }
            dashTimeLeft = dashTime;
            dashCDLeft = dashCD;
        }
        if(dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
        }
        if(dashCDLeft > 0)
        {
            dashCDLeft -= Time.deltaTime;
        }*/
        if(knockBackTimeLeft > 0)
        {
            knockBackTimeLeft -= Time.deltaTime;
        }
        if (!isWallJumping && !grappling && knockBackTimeLeft <= 0)
        {
            rb.velocity = new Vector2(horizontalV, rb.velocity.y);
        }
        if(Input.GetKey(KeyCode.S) && !grappling)
        {
            rb.velocity = new Vector2(rb.velocity.x, fastFallSpeed);
        } 
        else if(rb.velocity.y < 0 && !grappling)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, fastFallSpeed/2, 50));
        }

        //ANIMATION
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Walled", isWallSliding && ((isFacingRight && horizontal > 0) || (!isFacingRight && horizontal < 0)));
        anim.SetFloat("XSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("YSpeed", (rb.velocity.y));


        
    }


    private bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        closeToGround = Physics2D.Raycast(groundCheck.position, Vector2.down, 1.5f, groundLayer);
        return grounded;
    }

    private bool IsWalled()
    {
        walled = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        return walled;
    }

    private void WallSlide()
    {
        if (wallJumpEnabled && walled && !closeToGround && horizontal != 0f && !grappling && (rb.velocity.y <= 0 || shouldLatch > 0))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(0, -Mathf.Abs(Mathf.Clamp(rb.velocity.y, wallSlidingSpeed, float.MaxValue)));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            if (grappling)
            {
                hook.TryRetractHook();
                grappling = false;
            }
            extraJumpsLeft = extraJumps;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void KnockBack(Vector2 dir, float kbForce, float time = .5f) {
        rb.AddForce(dir * kbForce, ForceMode2D.Impulse);
        knockBackTimeLeft = time;
        if (grappling)
        {
            hook.TryRetractHook();
            grappling = false;
        }
    }

    //public void KnockBack(Vector2 dir, float kbX, float kbY) {
        // if (kbDir == 0) {
        //     rb.velocity = new Vector2(0, rb.velocity.y + kbY);
        // } else {
        //     rb.velocity = new Vector2(kbDir * kbX, rb.velocity.y + kbY);
        // }

        //rb.velocity = new Vector2(kbDir * kbX, rb.velocity.y + kbY);
    //}
}

