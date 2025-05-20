using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentPlayerState;

    public void Initialize(PlayerState startingState)
    {
        currentPlayerState = startingState;
        currentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState state)
    {
        currentPlayerState.ExitState();
        currentPlayerState = state;
        currentPlayerState.EnterState();
    }
}
