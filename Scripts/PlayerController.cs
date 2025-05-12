using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool isTalking = false;
    private CanvasGroup canvasGroup;
    private string[] lines;
    public Dialogue dialogue;
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
    public float ray;

    void Start()
    {
        canvasGroup = dialogue.gameObject.GetComponent<CanvasGroup>();
        victory.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
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

        isGrounded = Physics.Raycast(new Vector3(transform.position.x,transform.position.y-4,transform.position.z), Vector3.down, out hit, ray);



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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dialogue1"))
        {
            isTalking = true;
            Destroy(other.gameObject);
            lines = new string[3];
    
            lines[0] = "ciao";
            lines[1] = "ecco volevo dirti";
            lines[2] = "suicidati";

            dialogue.setLines(lines);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            dialogue.StartDialogue();
        }
    }

    public void setTalkingState(bool value)
    {
        isTalking = value;
    }
}
