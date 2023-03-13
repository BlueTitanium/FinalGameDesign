using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    [Header("Movement")]
    private float horizontal;
    [SerializeField]
    private float speed = 8f;
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

    /*
    [Header("LedgeClimb")]
    public bool ledgeDetected = false;
    [SerializeField] private LedgeDetection ledge;
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBeginPos;
    private Vector2 climbEndPos;
    private bool canGrabLedge = true, canClimb;
    private bool canClimbOver = false;
    */

    [Header("Assign Value")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    
    public void TakeDamage(float amount)
    {

    }
    public void Die()
    {

    }
    public void TakeHeal(float amount)
    {

    }



    private void Start()
    {
        p = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        IsGrounded(); IsWalled();

        if(dashTimeLeft<=0)
            horizontal = Input.GetAxisRaw("Horizontal");


        //if we decide on ledge climbing? idk it kinda looks wack with no animations not worth pursuing till then
        //checkForLedge();
        //if (canClimbOver)
        //{
        //    if((horizontal > 0 && isFacingRight) || (horizontal < 0 && !isFacingRight))
        //    {
        //        ledgeClimbOver();
        //    }
        //    else if((horizontal < 0 && isFacingRight) || (horizontal > 0 && !isFacingRight) || Input.GetButtonDown("Jump"))
        //    {
        //        canClimb = false;
        //        canClimbOver = false;
        //        Invoke("ResetCanGrabLedge", .1f);
        //    }
        //}

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
            } else
            {
                if(extraJumpsLeft > 0)
                {
                    extraJumpsLeft -= 1;
                    rb.velocity = new Vector2(rb.velocity.x, extraJumpPower);
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

        if (!isWallJumping)
        {
            rb.velocity = ((dashTimeLeft> 0) ? (new Vector2(horizontal * (speed + dashSpeed), rb.velocity.y)) : (new Vector2(horizontal * speed, rb.velocity.y)));
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10000, fastFallSpeed));
        } else
        if(rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    //private void checkForLedge()
    //{
    //    if(ledgeDetected && canGrabLedge)
    //    {
    //        canGrabLedge = false;
    //        Vector2 ledgePos = ledge.transform.position;
    //        climbBeginPos = ledgePos + new Vector2(offset1.x * (isFacingRight ? 1 : -1), offset1.y);
    //        climbEndPos = ledgePos + new Vector2(offset2.x * (isFacingRight ? 1 : -1), offset2.y);
    //        canClimb = true;
    //        Invoke("AllowClimbOver", .3f);
    //    }
    //    if (canClimb)
    //    {
    //        transform.position = climbBeginPos;
    //    }
    //}

    //private void ledgeClimbOver()
    //{
    //    canClimbOver = false;
    //    canClimb = false;
    //    transform.position = climbEndPos;
    //    Invoke("ResetCanGrabLedge", .1f);
    //}
    //private void AllowClimbOver() => canClimbOver = true;
    //private void ResetCanGrabLedge() => canGrabLedge = true;

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
        if (walled && !grounded && horizontal != 0f)
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
}
