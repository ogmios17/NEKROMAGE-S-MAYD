using UnityEngine;

public class ProjectileShooter :  MonoBehaviour, Shooter
{
    public GameObject player;
    public bool isActive;
    StateMachine stateMachine;
    private Animator animator;
    public GameObject projectile;
    public float distanceActivation;
    public float bulletRechargeTime;
    private float bulletRechargeTimer;
    public Transform firePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();

        
        stateMachine = new StateMachine();

        var shootingState = new ShootingState(this, animator);
        var voidState = new VoidState(this, animator);

        At(voidState,shootingState, new FuncPredicate(() => isActive && Vector3.Distance(player.transform.position, gameObject.transform.position) <= distanceActivation));
        At(shootingState, voidState, new FuncPredicate(() => Vector3.Distance(player.transform.position, gameObject.transform.position) >= distanceActivation));


        stateMachine.SetState(voidState);
    }
    void Start()
    {
        bulletRechargeTimer = 0; ;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
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
            GameObject projectileInstance = GameObject.Instantiate(projectile, firePoint.position, projectile.transform.rotation);
        }
    }

    void At(StateInterface from, StateInterface to, Predicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(StateInterface to, Predicate condition) => stateMachine.AddAnyTransition(to, condition);
}
