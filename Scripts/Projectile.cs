using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public ProjectileShooter shooter;
    private float timeToLive = 20;
    private float theReaper = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theReaper += Time.deltaTime;
        transform.position += shooter.transform.forward * Time.deltaTime * speed;
        if (theReaper >= timeToLive) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        //do other stuff later
    }
}
