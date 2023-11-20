using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerStates currentState {  get; private set; }

    public void Init(PlayerStates _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerStates newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
