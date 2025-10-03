using UnityEngine;

public abstract class BaseState:StateInterface
{
    protected readonly Controller controller;
    protected readonly Animator animator;

    protected BaseState(Controller controller, Animator animator)
    {
        this.controller = controller;
        this.animator = animator;
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}

