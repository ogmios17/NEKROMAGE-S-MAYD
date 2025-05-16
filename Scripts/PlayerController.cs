using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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

        if (Input.GetKeyDown(rand.GetJump()) && isGrounded && !isTalking) 
        {
            jumpRegistered = true;
        }

        if (gameObject.transform.position.y < 5) 
            SceneManager.LoadScene("SampleScene");
        Debug.Log("Grounded: " + isGrounded);
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
        if (jumpRegistered && isGrounded)
            Jump();
            
            

        HandleVelocity();

        for(int i = 0; i<5; i++){
            Vector3 origin = new Vector3(transform.position.x,transform.position.y-4,transform.position.z+(i-2)-1);
            rayLines[i].SetPosition(0, origin);
            rayLines[i].SetPosition(1, origin + Vector3.down*ray);
            if (Physics.Raycast(origin, Vector3.down, out hit, ray)){
                isGrounded = true;
                break;
            }
            isGrounded = false;
        }
        



    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(0f, 0f, rb.linearVelocity.z);
        rb.linearVelocity= new Vector3(0f,jumpForce,rb.linearVelocity.z);
        jumpRegistered = false;
        isGrounded = false;
    }

    string GetName(KeyCode key)
    {
        if (key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9)
        {
            return ((int)key - (int)KeyCode.Alpha0).ToString();
        }
        return key.ToString();

    }

    void HandleVelocity()
    {
        if (Mathf.Abs(rb.linearVelocity.z) > maxSpeed)
        {            
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, Mathf.Sign(rb.linearVelocity.z) * maxSpeed);
        }

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
}
