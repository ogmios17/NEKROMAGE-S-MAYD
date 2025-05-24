using UnityEngine;

public class Bounce : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody rb;
    public float bounceForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            rb = collision.gameObject.GetComponent<Rigidbody>();
            playerController.Jump(bounceForce);
        }
    }
}
