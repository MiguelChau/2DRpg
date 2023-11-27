using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represente o estados do jogador
public class PlayerStateMachine
{
    public PlayerStates currentState {  get; private set; } //state atual do jogador

    public void Init(PlayerStates _startState) //inicia a maquina de estados com um estado inicial
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerStates newState) //muda o estado atual para um novo estado
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
