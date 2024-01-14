using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffectStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Chamado quando algo entra no colisor. Realiza danos m�gicos ao inimigo.
        if(collision.GetComponent<Enemy>() != null) //verifica se o objecto colidid � um inimigo
        {
            //obt�m as estatisticas do jogador e do inimigo alvo
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget); //aplica dano magico do jogador ao inimigo
        }
    }

}
