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
    private bool duplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;



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
    public void SetupClone(Transform _newTransform, float _cloneDur, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _duplicate, float _chanceToDuplicate) //configura a posiçao do clone
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDur;

        duplicateClone = _duplicate;
        chanceToDuplicate = _chanceToDuplicate;
        closestEnemy = _closestEnemy;
        FaceCloseTarget();
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

                if(duplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceCloseTarget() //ortaciona o clone para enfrentar o inimigo mais perto
    {
        if(closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
