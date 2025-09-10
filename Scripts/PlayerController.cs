using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public TurnSprite turnSpriteScript;
    private Animator animator;
    private enum MoveDir { None, Forward, Backward }
    private MoveDir currentDirection = MoveDir.Forward;
    private Vector3 frontDirection;
    private Vector3 backDirection;
    [Tooltip("How fast the player will move")]
    public float force;
    [Tooltip("The value is subtracted form the horizontal force when not grounded")]
    public float airFriction;
    [Range(0f, 100f)]
    [Tooltip("The percentage of momentum carried from the previous movement")]
    public float airMomentumModifier;
    [Tooltip("Defines the max velocity at which the player can fall. To avoid too big accelerations")]
    [Min(0f)]
    public float maxFallingSpeed;
    [Tooltip("The player will be boosted downwards at the apex of the jump by this value")]
    public float peakBoostY;
    [Tooltip("The player will be boosted horizontally at the apex of the jump by this value")]
    public float peakBoostZ;
    [Tooltip("How high the player will jump")]
    public float jumpForce;
    [Tooltip("Friction on the ground")]
    public float drag;
    [Tooltip("The script that randomizes the input")]
    public InputRandomizer rand;
    [Tooltip("Max speed on the ground")]
    public float maxSpeed = 5;
    public GameObject victory;
    private RaycastHit hit;
    [Tooltip("How long the raycast for the ground check is")]
    public float ray;
    private float coyoteTimer;
    [Tooltip("This is the coyoteTime. Players will be able to jump even when not grounded if they are falling by this specific time")]
    public float floatingTime;
    private float groundCheckCooldown;
    private float groundCheckDelay = 0.1f;
    [Tooltip("Players will be able to jump if they become grounded after this time and are currently airborne")]
    public float jumpBufferTime;
    private float jumpBufferTimer = 0;
    private bool buffered;
    private float actualForce;
    private Animation animations;
    private float previousVelocity = 0;
    [Tooltip("Determines how abruptly the jump is interrupted if the player stops pressing the input. A value of 1 disables the modular jump feature")]
    [Min(1f)]
    public float modularJumpModifier;
    private bool isInteractable = true;
    private bool jumpRegistered;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool bouncing = false;
    private bool groundJustTouched = false;
    private float bufferJumpDelay = 0.1f;
    public CameraHandler cameraHandler;
    private bool isTalking = false;
    private Vector3 gravity;
    public float dashForce;
    public bool canDash;
    private bool hasDashed = false;
    private bool isDashing = false;
    public float dashingDuration;
    private bool dashRegistered;
    public float dashCooldown;
    private float dashCooldownTimer;
    [Tooltip("Determines how much the player has to wait before dashing again on the ground")]
    void Start()
    {
        dashCooldownTimer = dashCooldown;
        animations = gameObject.GetComponent<Animation>();
        animator = gameObject.GetComponent<Animator>();
        backDirection = Vector3.back;
        frontDirection = Vector3.forward;
        victory.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = new Vector3(0, -150, 0);

    }

    void Update()
    {
        if (!isInteractable)
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(rand.GetJump())) //jumping
            if ((isGrounded || (coyoteTimer > 0 && coyoteTimer < floatingTime)) && isInteractable) jumpRegistered = true;
            else if (isInteractable)
            {
                buffered = true;
                jumpBufferTimer = 0;
            }
        if (Input.GetKeyDown(KeyCode.S) && canDash && !hasDashed) //dashing
        {
            dashRegistered = true;
        }

        if (Input.GetKeyUp(rand.GetJump()) && !bouncing && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y / modularJumpModifier, rb.linearVelocity.z);
        }

        if (buffered) jumpBufferTimer += Time.deltaTime;

        if (gameObject.transform.position.y < 5)
            SceneManager.LoadScene("SampleScene");

        HandleCoyote();
        groundCheckCooldown -= Time.deltaTime;
        if (hasDashed)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
        
        
    }

    void FixedUpdate()
    {

        if(!isDashing) HandleVelocity();
        HandleControls();

        if (groundJustTouched)
        {
            bufferJumpDelay -= Time.deltaTime;
        }

        if (groundCheckCooldown <= 0)
        {
            GroundCheck();
        }

        actualForce = force;

    }

    void GroundCheck()
    {

        if (Physics.BoxCast(transform.position - new Vector3(0, 4, 0), new Vector3(0.25f, 0.1f, 0.25f), Vector3.down, out hit, Quaternion.identity, 1f))
        {
            Physics.gravity = gravity;
            if (dashCooldownTimer <= 0)
            {
                hasDashed = false;
                dashCooldownTimer = dashCooldown;
            }
            groundJustTouched = true;
            if (!hit.collider.CompareTag("Bounce"))
            {
                isGrounded = true;
                bouncing = false;
                coyoteTimer = 0;
                if (jumpBufferTimer > 0 && jumpBufferTimer < jumpBufferTime && bufferJumpDelay < 0)
                {
                    Jump(jumpForce);
                    buffered = false;
                    jumpBufferTimer = 0;
                }

            }
        }
        else
        {
            isGrounded = false;
            groundJustTouched = false;
            bufferJumpDelay = 0;
        }
    }
    public void Jump(float jump)
    {
        rb.linearVelocity = new Vector3(0f, Mathf.Min(0,rb.linearVelocity.y), 0f);
        hasDashed = false;
        rb.AddForce(new Vector3(0f, jump, 0f), ForceMode.Impulse);
        jumpRegistered = false;
        isGrounded = false;
        coyoteTimer = -1;
        groundCheckCooldown = groundCheckDelay;
    }

    IEnumerator Dash()
    {
        
        if (currentDirection == MoveDir.Forward)
        {
            rb.linearVelocity = frontDirection*dashForce;
        }
        else rb.linearVelocity = backDirection * dashForce;

        yield return new WaitForSeconds(dashingDuration);

        Physics.gravity = gravity;
        isDashing = false;

    }

    void HandleControls()
    {
        if (isDashing || !isInteractable) return;
        if (Input.GetKey(rand.GetBack()))
        {
            animator.SetBool("isRunning", true);
            rb.AddForce(frontDirection * actualForce, ForceMode.Impulse);
            if (currentDirection != MoveDir.Forward)
            {
                currentDirection = MoveDir.Forward;
                transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
                if (turnSpriteScript != null)
                {
                    turnSpriteScript.SetBaseRotation(transform.rotation.eulerAngles);
                }
            }
            cameraHandler.AdjustFront();
        }
        else if (Input.GetKey(rand.GetForward()))
        {
            animator.SetBool("isRunning", true);
            rb.AddForce(backDirection * actualForce, ForceMode.Impulse);
            if (currentDirection != MoveDir.Backward)
            {
                currentDirection = MoveDir.Backward;
                transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
                if (turnSpriteScript != null)
                {
                    turnSpriteScript.SetBaseRotation(transform.rotation.eulerAngles);
                }
            }

            cameraHandler.AdjustBack();
        }else
        {
            if (Mathf.Abs(rb.linearVelocity.z) > 0.5 || Mathf.Abs(rb.linearVelocity.x) > 0.5)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x * airMomentumModifier / 100, rb.linearVelocity.y, rb.linearVelocity.z * airMomentumModifier / 100);
            animator.SetBool("isRunning", false);
            cameraHandler.ResetOffsets();
        }


        if (dashRegistered)
        {
            dashRegistered = false;
            isDashing = true;
            hasDashed = true;
            Physics.gravity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(Dash());
        }

        if (jumpRegistered && (isGrounded || (coyoteTimer < floatingTime && coyoteTimer > 0)))
            Jump(jumpForce);
    }
    void HandleVelocity()
    {

        if (Mathf.Abs(rb.linearVelocity.z) > maxSpeed)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, Mathf.Sign(rb.linearVelocity.z) * maxSpeed);
        }
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector3(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y, 0);
        }
        
        if (previousVelocity > 0 && rb.linearVelocity.y < 0 && coyoteTimer < 0)              //handles peak boost
        {
            Physics.gravity = new Vector3(0, peakBoostY, 0);
  
        }
        if (rb.linearVelocity.y < -maxFallingSpeed)                 //handles max falling speed
        {
            rb.linearVelocity = new Vector3(0, Mathf.Sign(rb.linearVelocity.y) * maxFallingSpeed, rb.linearVelocity.z);
        }
        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.linearDamping = 0;
            actualForce = force - airFriction;
        }
        previousVelocity = rb.linearVelocity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            victory.SetActive(true);
            
        }
    }



    public void setInteractableState(bool value)
    {
        isInteractable = value;
        if (value == false)
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        else if (frontDirection.z > 0)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        Debug.Log(value);
    }
    public bool IsInteractable()
    {
        return isInteractable;
    }
    public void HandleCoyote()
    {
        if (!isGrounded && coyoteTimer >= 0)
        {
            coyoteTimer += Time.deltaTime;
        }
    }

    public bool GetGroundedState()
    {
        return isGrounded;
    }

    public void setFrontDirection(Vector3 direction)
    {
        frontDirection = direction;
    }

    public void setBackDirection(Vector3 direction)
    {
        backDirection = direction;
    }

    public Vector3 getFrontDirection()
    {
        return frontDirection;
    }

    public Vector3 getBackDirection()
    {
        return backDirection;
    }

    public void SetBouncing(bool bouncing)
    {
        this.bouncing = bouncing;
    }

    public void setTalkingState(bool value)
    {
        this.isTalking = value;
    }

    public bool IsTalking()
    {
        return isTalking;
    }

}

