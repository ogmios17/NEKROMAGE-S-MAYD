using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float force;
    public float jumpForce;
    public float drag;
    private bool jumpRegistered;
    private Rigidbody rb;
    private bool isGrounded = true;
    public InputRandomizer rand; 
    public TextMeshProUGUI backText;
    public TextMeshProUGUI forwardText;
    public TextMeshProUGUI jumpText;
    public float maxSpeed = 5;
    public float gravity = 10;
    public GameObject victory;

    public GameObject q, w, e, r, t, y, u, i, o, p, a, s, d, f, g, 
            h, j, k, l, z, x, c, v, b, n, m, zero, one, two, three, four, 
            five, six, seven, eight, nine, space, up, down, left, right;



    void Start()
    {
        victory.SetActive(false);
        GameObject[] keyImages =
        {
            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
            zero, one, two, three, four, five, six, seven, eight, nine, space, up, down, left, right
        };
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        backText.text = GetName(rand.GetBack());
        forwardText.text = GetName(rand.GetForward());
        jumpText.text = GetName(rand.GetJump());
        if (Input.GetKeyDown(rand.GetJump()) && isGrounded)
        {
            jumpRegistered = true;
        }

        if (gameObject.transform.position.y < 5) 
            SceneManager.LoadScene("SampleScene");
    }

    void FixedUpdate()
    {

        if (isGrounded)
        {
            rb.linearDamping = drag;
        }
        else rb.linearDamping = 0;
        if (Input.GetKey(rand.GetBack()))
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
        if (Input.GetKey(rand.GetForward()))
            rb.AddForce(Vector3.back * force, ForceMode.Impulse);
        if (jumpRegistered && isGrounded)
            Jump();

        HandleVelocity();
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(0f, 0f, rb.linearVelocity.z);
        rb.linearVelocity= new Vector3(0f,jumpForce,rb.linearVelocity.z);
        isGrounded = false;
        jumpRegistered = false;
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
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            victory.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
