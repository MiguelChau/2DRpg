using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyStates currentState {  get; private set; }

    public void Init( EnemyStates _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyStates _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
