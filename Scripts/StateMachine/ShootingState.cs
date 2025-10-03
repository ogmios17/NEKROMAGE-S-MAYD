using UnityEngine;

public class ShootingState : BaseState
{
    private Shooter shooter;
    public ShootingState(Shooter controller, Animator animator) : base(controller, animator)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Entered state");
        shooter = (Shooter)controller;
    }

    public override void Update()
    {
        shooter.Shoot();
    }
}
