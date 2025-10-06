using UnityEngine;

public class RandomReset : MonoBehaviour
{
    public InputRandomizer randomizer;
    public bool resetJump;
    public bool resetForward;
    public bool resetBack;
    private Vector3 position;
    private float respawnTime;
    private float respawnTimer;
    public float rallentyTime;
    private bool timerActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnTime = 3;
        respawnTimer = respawnTime;
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            respawnTimer -= Time.deltaTime;
        }
        if (respawnTimer <= 0)
        {
            timerActive = false;
            respawnTimer = respawnTime;
            GameObject.Instantiate(gameObject, position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (respawnTimer < respawnTime - rallentyTime)
        {
            Time.timeScale = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Reset();         
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Reset();
            Time.timeScale = 0.3f;
        }
        gameObject.transform.position = new Vector3(10000, 10000, 10000);
        timerActive = true;
    }

    private void Reset()
    {
        if (resetJump)
        {
            randomizer.setDefaultJump();           
        }
        if (resetForward)
        {
            randomizer.setDefaultForward();
        }
        if (resetBack)
        {
            randomizer.setDefaultBack();
        }
    }
}
