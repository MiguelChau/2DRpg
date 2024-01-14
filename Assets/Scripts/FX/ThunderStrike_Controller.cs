using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats; //estatisticas do alvo 
    [SerializeField] private float speed; //velocidade do ataque thunder
    private int damage;

    private Animator anim;
    private bool triggered;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats) //confirgura o dano e as estatiscticas do alvo
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void Update() //responsavel por mover o ataque em dira�ao ao alvo e realiza a�oes quando proximo
    {
        if (!targetStats)
            return;
        
        if (triggered)
            return;

        //move o ataque em dira�ao do alvo
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        //quando o proximo ao alvo:
        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            //ajusta a posi�ao e a escala
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageSelfDestroy", .2f); //invoca a destrui�ao do ataque 
            triggered = true;           
            anim.SetTrigger("Hit"); //ativa a anim�ao de hit
            
        }
    }

    private void DamageSelfDestroy() //aquando aplica o efeito , causa dano e destroi o objeto
    {
        targetStats.ApplyShockAilment(true); //aplica o efeito ao alvo
        targetStats.TakeDamage(damage); //causa o dano ao alvo
        Destroy(gameObject, .4f);
    }
}
