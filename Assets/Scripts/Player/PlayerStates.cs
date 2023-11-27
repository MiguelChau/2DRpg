using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script � uma classe para todos os estados do jogador
public class PlayerStates
{
    protected PlayerStateMachine stateMachine; //referente aos estados do jogador -> StateMachine
    protected Player player; //referencia ao joador

    protected Rigidbody2D rb;

    protected float xInput; //entradas do jogador nos eixos X e Y
    protected float yInput;
    private string animBoolName; //nome da booleana de anima��o associado a este estado

    protected float stateTimer; //temporizador 
    protected bool triggerCalled;
    public PlayerStates(Player _player, PlayerStateMachine _stateMachine, string animBoolName)//construtor que inicia as referencias do jogador, state machine e animbools.
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = animBoolName;   
    }

    public virtual void Enter() //Inicia quando o state � ativado. Anima��es e variaveis associadas.
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;

    }
    
    public virtual void Update() //Chamado a cada frame, atualizada temporizadores, entrada e a�oes do jogador
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit() //Quando o estado � deixado e desativa anima�ao associada
    {
        player.anim.SetBool(animBoolName, false);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
