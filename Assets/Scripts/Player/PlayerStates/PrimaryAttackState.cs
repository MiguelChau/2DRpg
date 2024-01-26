using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Representa o estado de ataque primario do jogador
public class PrimaryAttackState : PlayerStates
{

    public int comboCounter { get; private set; }             //Controlo de combo de ataques

    private float lastTimeAttacked; //Armazena o tempo do ultimo ataque
    private float comboWindow = 2; //Define a janela de tempo para realizar as combos
    public PrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        
        xInput = 0; //para resolver o bug do attack direction

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow) //Verifica se o combocounter é maior que 2 ou se ja passou o tempo da janela de combo
            comboCounter = 0; //Se sim, renicia o comboCounter

        player.anim.SetInteger("ComboCounter", comboCounter); //Configura o contador de combo no Animator (usado um subStateMachine)

      
        float attackDir = player.facingDir; //Direçao do ataque baseado na entrada do jogador, perimitindo que o ataque mude de diraçao durante a animaçao
        if(xInput != 0)
            attackDir = xInput; //para alterar a direçao do ataque durante o ataque

       
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y); //velocidade do ataque

        stateTimer = .1f;

    }

    public override void Exit()
    {
        base.Exit();
        
        player.StartCoroutine("BusyFor", .15f); //inicia a coroutine de BusyFor para tornar o jogador ocupado e evitando ataques consecutivos


        comboCounter++;  //incrementa o contador de combo
        lastTimeAttacked = Time.time; //atualiza o tempo do ultimo ataque
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) //reeduz o temporizador de estado
            player.SetZeroVelocity();   //zera a velocidade quando o tempo acaba , passado para o Enter do idlestate

        if (triggerCalled) //quando da trigger na ultima animaçao do ataque, passa para o idlestate
            stateMachine.ChangeState(player.idleState);
    }
}
