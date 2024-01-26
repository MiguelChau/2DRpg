using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostExperienceAmount = PlayerManager.instance.experience;
        PlayerManager.instance.experience = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();

    }

    protected override void DecreaseHealthBy(int _damage)
    {
        // Sobrescreve o método da classe base para adicionar comportamentos adicionais quando a saúde do jogador é reduzida.
        //Obtém o equipamento atual do tipo Armor do inventário e aplica seu efeito no jogador.
        base.DecreaseHealthBy(_damage);

        if (isDead)
            return;

        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockBackPower(new Vector2(10, 5));
            player.fx.ScreenShake(player.fx.shakeHighDamage);

            int randomSound = Random.Range(34, 35);
            AudioManager.Instance.PlaySFX(randomSound, null);
            
        }

        ItemDataEquipement currentArmor = Inventory.Instance.GetEquipment(EquipementType.Armor);

        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (AvoidAttacks(_targetStats)) //verifica se o ataque foi evitado
            return;

        int totalDamage = damage.GetValue() + strength.GetValue(); //calcula o dano total 

        if (_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CriticalChance()) //verifica se ocorreu um critical strike
        {
            totalDamage = CalculeCriticalDamage(totalDamage);
        }


        totalDamage = ArmorMethod(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);
    }
}
