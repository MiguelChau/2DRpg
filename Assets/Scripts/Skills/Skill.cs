using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsavel para todas as habilidades que vao herdar (herança).
public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("skill in CD");
        return false;
    }

    public virtual void UseSkill()
    {
        //Do some skill specific 
    }
}
