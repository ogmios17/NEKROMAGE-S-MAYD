using UnityEngine;

public class LaserShooter : MonoBehaviour, Shooter
{
    public GameObject player;
    public bool isActive;
    StateMachine stateMachine;
    private Animator animator;
    public float distanceActivation;
    public float bulletRechargeTime;
    private float bulletRechargeTimer;
    public float laserDuration;
    private float laserDurationTimer;
    private bool laserActive = false;
    public Transform firePoint;
    public LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();


        stateMachine = new StateMachine();

        var shootingState = new ShootingState(this, animator);
        var voidState = new VoidState(this, animator);

        At(voidState, shootingState, new FuncPredicate(() => isActive && Vector3.Distance(player.transform.position, gameObject.transform.position) <= distanceActivation));
        At(shootingState, voidState, new FuncPredicate(() => Vector3.Distance(player.transform.position, gameObject.transform.position) >= distanceActivation));


        stateMachine.SetState(voidState);
    }
    void Start()
    {
        bulletRechargeTimer = 0;
        //lineRenderer.enabled = false;

        lineRenderer.positionCount = 2; // ELIMINA DOPO IL DEBUG!
        lineRenderer.startWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRenderer.endWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // ELIMINA DOPO IL DEBUG!
        lineRenderer.startColor = Color.green; // ELIMINA DOPO IL DEBUG!
        lineRenderer.endColor = Color.green; // ELIMINA DOPO IL DEBUG!
        lineRenderer.enabled = true; // ELIMINA DOPO IL DEBUG!
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        if (laserActive)
        {
            laserDurationTimer += Time.deltaTime;
        }
        if (laserDurationTimer >= laserDuration)
        {
            laserActive = false;
            laserDurationTimer = 0;
            lineRenderer.enabled = false;
        }
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    public void Shoot()
    {
        bulletRechargeTimer -= Time.deltaTime;
        if (bulletRechargeTimer <= 0)
        {
            Debug.Log("Sparo!");
            bulletRechargeTimer = bulletRechargeTime;
            lineRenderer.enabled = true;
            laserActive = true;
        }
    }

    void At(StateInterface from, StateInterface to, Predicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(StateInterface to, Predicate condition) => stateMachine.AddAnyTransition(to, condition);
}
