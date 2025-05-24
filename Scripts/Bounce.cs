using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 15f;
    private float timer = 0.05f;
    private bool activateTimer;
    private Rigidbody rb;
    private PlayerController playerController;
    public void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public void Update()
    {
        if (activateTimer)
        {
            timer -= Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {
        if (timer < 0)
        {
            activateTimer = false;
            timer = 0.05f;
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            playerController.SetBouncing(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

                activateTimer = true;
                
            
        }
    }
}



// nel caso qualsuno si stesse chiedendo perchè questa meccanica sia stata implementata da qualcuno che sembra sotto effetto di sostanze, è colpa di unity <3