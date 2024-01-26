using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsavel para todas as habilidades que vao herdar (heran�a).
public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        CheckUnlock();
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreatePopUp("Cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //Do some skill specific 
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTrasform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTrasform.position, 25);

        float closestDis = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTrasform.position, hit.transform.position);

                if (distanceToEnemy < closestDis)
                {
                    closestDis = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

        }
        return closestEnemy;
    }
}
