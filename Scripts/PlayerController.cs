using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float previousVelocity=0;
    public float maxFallingSpeed;
    public float peakBoostY;
    public float peakBoostZ;
    private bool isTalking = false;
    public float force;
    public float jumpForce;
    public float drag;
    private bool jumpRegistered;
    private Rigidbody rb;
    private bool isGrounded = false;
    public InputRandomizer rand; 
    public float maxSpeed = 5;
    public float gravity = 10;
    public GameObject victory;
    private RaycastHit hit;
    private float raycastSpread = 1;
    public float ray;
    private float coyoteTimer;
    public float floatingTime;
    private float groundCheckCooldown;
    private float groundCheckDelay = 0.1f;
    private bool boosted;
    public TextMeshProUGUI timer = null;    //DELETE AFTER DEBUG

    private List<LineRenderer> rayLines = new List<LineRenderer>();  //DFELETE AFTER DEBUG

    void Start()
    {
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
        timer.text = previousVelocity.ToString();    //DELETE AFTER DEBUG
        if (Input.GetKeyDown(rand.GetJump()))
            if ((isGrounded || (coyoteTimer > 0 && coyoteTimer < floatingTime))&&!isTalking) jumpRegistered = true;
        

        if (gameObject.transform.position.y < 5) 
            SceneManager.LoadScene("SampleScene");
        Debug.Log("Grounded: " + isGrounded);

        HandleCoyote();
        groundCheckCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {

        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else rb.linearDamping = 0;
        if (Input.GetKey(rand.GetBack()) && !isTalking) 
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
        if (Input.GetKey(rand.GetForward()) && !isTalking)  
            rb.AddForce(Vector3.back * force, ForceMode.Impulse);
        if (jumpRegistered && (isGrounded || (coyoteTimer<floatingTime && coyoteTimer>0)))
            Jump();
            
            

        HandleVelocity();
        if (groundCheckCooldown <= 0)
        {
            GroundCheck();
        }



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
                break;
            }
            if (i == 4) isGrounded = false;
        }
    }
    void Jump()
    {
        rb.linearVelocity = new Vector3(0f, 0f, rb.linearVelocity.z);
        rb.linearVelocity= new Vector3(0f,jumpForce,rb.linearVelocity.z);
        jumpRegistered = false;
        isGrounded = false;
        coyoteTimer = -1;
        groundCheckCooldown = groundCheckDelay;
    }

    void HandleVelocity()
    {
        if (Mathf.Abs(rb.linearVelocity.z) > maxSpeed)
        {            
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, Mathf.Sign(rb.linearVelocity.z) * maxSpeed);
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

    

    public void setTalkingState(bool value)
    {
        isTalking = value;
    }
    public void HandleCoyote()
    {
        if(!isGrounded && coyoteTimer >=0)
        {
            coyoteTimer+= Time.deltaTime;
        }
    }
}
