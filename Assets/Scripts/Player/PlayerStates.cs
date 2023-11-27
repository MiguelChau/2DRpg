using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script é uma classe para todos os estados do jogador
public class PlayerStates
{
    protected PlayerStateMachine stateMachine; //referente aos estados do jogador -> StateMachine
    protected Player player; //referencia ao joador

    protected Rigidbody2D rb;

    protected float xInput; //entradas do jogador nos eixos X e Y
    protected float yInput;
    private string animBoolName; //nome da booleana de animação associado a este estado

    protected float stateTimer; //temporizador 
    protected bool triggerCalled;
    public PlayerStates(Player _player, PlayerStateMachine _stateMachine, string animBoolName)//construtor que inicia as referencias do jogador, state machine e animbools.
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = animBoolName;   
    }

    public virtual void Enter() //Inicia quando o state é ativado. Animações e variaveis associadas.
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;

    }
    
    public virtual void Update() //Chamado a cada frame, atualizada temporizadores, entrada e açoes do jogador
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit() //Quando o estado é deixado e desativa animaçao associada
    {
        player.anim.SetBool(animBoolName, false);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
