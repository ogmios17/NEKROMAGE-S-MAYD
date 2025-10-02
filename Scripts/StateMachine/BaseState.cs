using UnityEngine;

public abstract class BaseState:StateInterface
{
    protected readonly PlayerController player;
    protected readonly Animator animator;

    protected BaseState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}

