using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private enum MoveDir { None, Forward, Backward }
    private MoveDir currentDirection = MoveDir.Forward;
    private Vector3 frontDirection;
    private Vector3 backDirection;
    public float airFriction;
    private float previousVelocity=0;
    public float maxFallingSpeed;
    public float peakBoostY;
    public float peakBoostZ;
    private bool isInteractable = true;
    public float force;
    public float jumpForce;
    public float drag;
    private bool jumpRegistered;
    private Rigidbody rb;
    private bool isGrounded = false;
    public InputRandomizer rand; 
    public float maxSpeed = 5;
    public GameObject victory;
    private RaycastHit hit;
    private float raycastSpread = 1;
    public float ray;
    private float coyoteTimer;
    public float floatingTime;
    private float groundCheckCooldown;
    private float groundCheckDelay = 0.1f;
    private bool boosted;
    public float jumpBufferTime;
    private float jumpBufferTimer = 0;
    private bool buffered;
    private float actualForce;
    public Animation animations;
    public TextMeshProUGUI timer = null;    //DELETE AFTER DEBUG

    private List<LineRenderer> rayLines = new List<LineRenderer>();  //DFELETE AFTER DEBUG

    void Start()
    {
        backDirection = Vector3.back;
        frontDirection = Vector3.forward;
        victory.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();

        for (int i = 0; i < 5; i++)     //DELETE AFTER DEBUG
        {
            GameObject lineObj = new GameObject("RayLine_" + i);
            lineObj.transform.parent = this.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.startColor = Color.green;
            lr.endColor = Color.red;

            rayLines.Add(lr);
        }
    }


    void Update()
    {
        timer.text = jumpBufferTimer.ToString();    //DELETE AFTER DEBUG
        if (Input.GetKeyDown(rand.GetJump()))
            if ((isGrounded || (coyoteTimer > 0 && coyoteTimer < floatingTime)) && isInteractable) jumpRegistered = true;
            else buffered = true;

        if (buffered) jumpBufferTimer += Time.deltaTime;

        if (gameObject.transform.position.y < 5)
                SceneManager.LoadScene("SampleScene");

        HandleCoyote();
        groundCheckCooldown -= Time.deltaTime;

        if (frontDirection.z > 0)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else { 
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; 
        }
    }

    void FixedUpdate()
    {

        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else
        {
            rb.linearDamping = 0;
            actualForce -= airFriction;
        }
        if (Input.GetKey(rand.GetBack()) && isInteractable)
        {
            rb.AddForce(frontDirection * actualForce, ForceMode.Impulse);
            if (currentDirection != MoveDir.Forward)
            {
                currentDirection = MoveDir.Forward;
                animations.Play("SwitchSide");
            }
        }
        else if (Input.GetKey(rand.GetForward()) && isInteractable)
        {
            rb.AddForce(backDirection * actualForce, ForceMode.Impulse);
            if (currentDirection != MoveDir.Backward)
            {
                currentDirection = MoveDir.Backward;
                animations.Play("SwitchSideBackwards");
            }
        }
        


        if (jumpRegistered && (isGrounded || (coyoteTimer < floatingTime && coyoteTimer > 0)))
            Jump();
            
            

        HandleVelocity();
        if (groundCheckCooldown <= 0)
        {
            GroundCheck();
        }

        actualForce = force;

    }

    void GroundCheck()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 origin = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z + (i - 2) - 1);
            rayLines[i].SetPosition(0, origin);
            rayLines[i].SetPosition(1, origin + Vector3.down * ray);
            if (Physics.Raycast(origin, Vector3.down, out hit, ray))
            {
                isGrounded = true;
                coyoteTimer = 0;
                boosted = false;
                if(jumpBufferTimer>0 && jumpBufferTimer < jumpBufferTime && rb.linearVelocity.y == 0)
                {
                    Jump();
                    jumpBufferTimer = 0;
                    buffered = false;
                }
                if(rb.linearVelocity.y == 0)
                {
                    jumpBufferTimer = 0;
                    buffered = false;
                }
                
                break;
            }
            if (i == 4) isGrounded = false;
        }
    }
    void Jump()
    {
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(new Vector3(0f, jumpForce,0f), ForceMode.Impulse);
        jumpRegistered = false;
        isGrounded = false;
        coyoteTimer = -1;
        groundCheckCooldown = groundCheckDelay;
    }

    void HandleVelocity()
    {

        if (!isGrounded)
        {
            //rb.AddForce(new Vector3(0, 0, Mathf.Sign(rb.linearVelocity.z)*airForce),ForceMode.Impulse);
        }
        if (Mathf.Abs(rb.linearVelocity.z) > maxSpeed)
        {            
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, Mathf.Sign(rb.linearVelocity.z) * maxSpeed);
        }
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector3(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y, 0);
        }
        if ((Mathf.Abs(rb.linearVelocity.y) > maxFallingSpeed))
        {
            rb.linearVelocity = new Vector3(0, Mathf.Sign(rb.linearVelocity.y) * maxFallingSpeed, rb.linearVelocity.z);
        }
        if(previousVelocity>0 && rb.linearVelocity.y<0 && coyoteTimer <0)
        {
            rb.AddForce(new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * peakBoostY, rb.linearVelocity.z * peakBoostZ));
            boosted = true;
        }
        previousVelocity = rb.linearVelocity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            victory.SetActive(true);
            Time.timeScale = 0;
        }
    }

    

    public void setInteractableState(bool value)
    {
        isInteractable = value;
    }
    public void HandleCoyote()
    {
        if(!isGrounded && coyoteTimer >=0)
        {
            coyoteTimer+= Time.deltaTime;
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
}
