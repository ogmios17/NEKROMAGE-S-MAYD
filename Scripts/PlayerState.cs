using UnityEngine;

public class PlayerState 
{
    protected PlayerController player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(PlayerController player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }
}
