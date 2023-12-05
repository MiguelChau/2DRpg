using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controla o comportamento de um clone criado pela habilidade do clone skill cs
public class Clone_SkillController : MonoBehaviour
{
    private SpriteRenderer sprite; //manipula a aparencia 
    private Animator anim; //animaçao do clone
    [SerializeField] private float colorLoosing;

    private float cloneTimer;
    [Space]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private Transform closestEnemy;



    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0 )
        {
            sprite.color = new Color(1, 1, 1, sprite.color.a - (Time.deltaTime * colorLoosing));

            if (sprite.color.a < 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDur, bool _canAttack) //configura a posiçao do clone
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        

        transform.position = _newTransform.position;
        cloneTimer = _cloneDur;
    }

    private void AnimationTrigger() //chamado por uma animaçao para destruir o clone
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger() //ativa um ataque ao redor do clone
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position,attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceCloseTarget() //ortaciona o clone para enfrentar o inimigo mais perto
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDis = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null) 
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if(distanceToEnemy < closestDis) 
                {
                    closestDis = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

        }

        if(closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
