using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsavel para todas as habilidades que vao herdar (heran�a).
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
