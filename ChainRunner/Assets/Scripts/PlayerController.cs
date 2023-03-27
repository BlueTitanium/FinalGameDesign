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
    [SerializeField]
    private float maxHP = 100f;
    private float curHP = 100f;

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
    [SerializeField] private float grappleSpeed = 30f;
    public bool grappling = false;
    private Vector2 grappleEndPoint;
    [SerializeField] private ChainHook hook;
    private float shouldLatch = 0f;

    [Header("Assign Value")]
    public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Animator anim;

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
        curHP -= amount;
        lerpTimer = 0f;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curHPText.text = ""+ curHP;
        if(curHP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {

    }
    public void TakeHeal(float amount)
    {
        curHP += amount;
        lerpTimer = 0f;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curHPText.text = "" + curHP;
    }

    public void GrappleToLocation(Vector2 dir, Vector2 point)
    {
        extraJumpsLeft = extraJumps;
        grappling = true;
        grappleEndPoint = point;
    }


    private void Start()
    {
        p = this;
        curHP = maxHP;
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
        
        if (!isWallJumping)
        {
            Flip();
        }

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
        }

        if (!isWallJumping && !grappling)
        {
            rb.velocity = new Vector2(horizontalV, rb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.S) && !grappling)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10000, fastFallSpeed));
        } else
        if(rb.velocity.y < 0 && !grappling)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        //ANIMATION
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Walled", walled);
        anim.SetFloat("XSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("YSpeed", (rb.velocity.y));
        
    }


    private bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return grounded;
    }

    private bool IsWalled()
    {
        walled = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        return walled;
    }

    private void WallSlide()
    {
        if (walled && !grounded && horizontal != 0f && !grappling && (rb.velocity.y <= 0 || shouldLatch > 0))
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

    public void KnockBack(Vector2 dir, float kbForce) {
        rb.AddForce(dir * kbForce, ForceMode2D.Impulse);
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

