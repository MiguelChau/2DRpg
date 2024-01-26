using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Health,
    Armor,
    Evasion,
    MagicRes,
    FireDamage,
    IceDamage,
    LightiningDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stats")]
    public Stat strength; //1 point increace damage by 1 and crit power by 1%
    public Stat agility; //1 point increase evasion by 1% nd crit chance by 1%
    public Stat intelligence; //1 point increas magic damage by 1 and magic resis by 3
    public Stat vitality; //1 point increase health by  5 points
    [Space]
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; //default value 150%
    [Space]
    [Header("Defensive Sats")]
    public Stat health;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    [Space]
    //Efeitos de status
    public bool isIgnited; //dmg overtime
    public bool isFrozen; //decrease armor
    public bool isShocked; //reduce accuracy by 20%

    //temporizadores para controle dos efeitos
    private float ignitedTimer;
    private float frozenTimer;
    private float shockedTimer;

    //temporiazorees para aplicaçao de dano ao longo do tmepo
    [SerializeField] private float ailmentsDur = 4;
    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int _currentHealth;

    public System.Action onHealthChange; //ação que sera acionada sempre que a life mudar

    public bool isDead {  get; private set; }
    public bool isInvencible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.DefaultValues(150); //valor padrao do poder crit
        _currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        frozenTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (frozenTimer < 0)
            isFrozen = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteAilment();
    }

    public void MakeVulnerable(float _dur) => StartCoroutine(VulnerableCoroutine(_dur));

    private IEnumerator VulnerableCoroutine(float _dur)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_dur);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify) //especie de flask wow ou phial. Usado para buffs temporarios
    {
        StartCoroutine(StartModifierCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StartModifierCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    //Responsavel por calcular e aplicar o dano ao alvo. Considera o dano fisico, a crit chance e reduçao do dano com base na armadura do alvo.
    public virtual void DoDamage(CharacterStats _targetStats) 
    {
        bool critStrike = false;

        if (_targetStats.isInvencible)
            return;

        if (AvoidAttacks(_targetStats)) //verifica se o ataque foi evitado
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue(); //calcula o dano total 

        if (CriticalChance()) //verifica se ocorreu um critical strike
        {
            totalDamage = CalculeCriticalDamage(totalDamage);
            critStrike = true;
        }

        fx.CreateHitFX(_targetStats.transform, critStrike);

        totalDamage = ArmorMethod(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);
    }

    #region Magical Damage and Elements 
    //lida com o calculo do dano magico e a aplicaçao de efeitos de elementos.
    public virtual void DoMagicDamage(CharacterStats _targetStats) //calcula o dano magico total e chama o metodo de applyailmentsfuction.
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue(); //calcula o dano total da magia


        totalMagicDamage = TargetResistance(_targetStats, totalMagicDamage); //aplica resistencia do alvo ao dano magico
        _targetStats.TakeDamage(totalMagicDamage); //causa dano ao alvo


        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0) //Se todos os tipos de dano mágico forem zero, não há necessidade de aplicar efeitos adicionais
            return; //isto serve para que se todos os valores forem 0, durante o while loop para sair desse loop, sem isto a Unity iria freezar e dar overload

        ApplyAilmentsFuction(_targetStats, _fireDamage, _iceDamage, _lightDamage);

    }

    // Aplica efeitos secundários com base no dano mágico causado
    private void ApplyAilmentsFuction(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightDamage)
    {
        //o maior dmg do element (value), é o vencedor do debuff aplicado
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightDamage;
        bool canApplyFrozen = _iceDamage > _fireDamage && _iceDamage > _lightDamage;
        bool canApplyShock = _lightDamage > _iceDamage && _lightDamage > _fireDamage;

        while (!canApplyIgnite && !canApplyFrozen && !canApplyShock) //qual tiver em primeiro, tem maior chance de calhar, neste caso o ignite
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);

                return;
            }

            if (Random.value < .45f && _iceDamage > 0)
            {
                canApplyFrozen = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);

                return;
            }

            if (Random.value < .5f && _lightDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);

                return;
            }

        }

        // Se for possível aplicar um debuff, configura os danos ao longo do tempo
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if (canApplyShock)
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightDamage * .1f));

        _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);
        //No que for maior é o que é aplicado
    }

    //responsavel por aplicar os efeitos de estado. Verifica se é possivel aplicar cada efeito, consoante o estado da personagem
    public void ApplyElements(bool _ignite, bool _frozen, bool _shock) 
    {
        // Verifica se é possível aplicar cada efeito secundário
        bool canApplyIgnite = !isIgnited && !isFrozen && !isShocked;
        bool canApplyFrozen = !isIgnited && !isFrozen && !isShocked;
        bool canApplyShock = !isIgnited && !isFrozen;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDur;

            fx.InvokeIgniteFx(ailmentsDur);
        }

        if (_frozen && canApplyFrozen)
        {
            frozenTimer = ailmentsDur;
            isFrozen = _frozen;

            // Reduz a velocidade do personagem ao congelar
            float _slowPercent = .2f;

            GetComponent<Entity>().SlowEntity(_slowPercent, ailmentsDur);
            fx.InvokeFrozenFx(ailmentsDur);
        }

        if (_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShockAilment(_shock);

            }
            else
            {
                // Se já estiver chocado, atinge o alvo mais próximo
                if (GetComponent<Player>() != null)
                    return;

                HitShockClosest();
            }

            //find closest target, only among enemies -> usando o metodo ja usado FindClosestEnemy
            // instantiate thunderstrike and setup it
        }

    }


    private void ApplyIgniteAilment()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (_currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void ApplyShockAilment(bool _shock)
    {
        if (isShocked)
            return;

        shockedTimer = ailmentsDur;
        isShocked = _shock;

        fx.InvokeShockFx(ailmentsDur);
    }

    private void HitShockClosest()
    {
        // Obtém todos os colisores próximos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDis = Mathf.Infinity;
        Transform closestEnemy = null;

        // Encontra o inimigo mais próximo
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDis)
                {
                    closestDis = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            // Se não encontrar nenhum inimigo, ataca a própria entidade
            if (closestEnemy == null) //se nao quisermos este disparo para o 2nd target, comentar estar duas linhas
                closestEnemy = transform;
        }

        if (closestEnemy != null)  // Cria e configura um ataque de choque
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());

        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockDamage(int _damage) => shockDamage = _damage;

    #endregion
    public virtual void TakeDamage(int _damage) // Método chamado ao receber dano físico
    {
        if (isInvencible)
            return;

        DecreaseHealthBy(_damage); // Reduz a saúde com base no dano físico

        GetComponent<Entity>().DamageImpact(); //em vez de termos separadamente no playerstats e enemystats colocamos este metodo que funciona por override
        fx.StartCoroutine("FlashFX");

        if (_currentHealth < 0 && !isDead)  // Verifica se o personagem morreu
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount) //usado tanto para aas healthpots como para o lifesteal da weapon
    {
        _currentHealth += _amount;

        if(_currentHealth > GetMaxHealthValue()) // Garante que a life não ultrapasse o máximo
            _currentHealth = GetMaxHealthValue();

        if(onHealthChange != null)
            onHealthChange();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.2f);
        }

        _currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUp(_damage.ToString());

        if(onHealthChange != null)
            onHealthChange();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void FallingDeadZone()
    {
        if (!isDead)
            Die();
    }

    public void InvencibleTime(bool _invencible) => isInvencible = _invencible;


    #region Stat calculation
    // Calcula o dano físico considerando a armadura do alvo
    protected int ArmorMethod(CharacterStats _targetStats, int totalDamage) //onde contem o armor stats e values e o efeito de frozen
    {
        if (_targetStats.isFrozen) // Se o alvo estiver congelado, reduz o dano com base na armadura
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // Garante que o dano não seja negativo
        return totalDamage;
    }

    private int TargetResistance(CharacterStats _targetStats, int totalMagicDamage) // Aplica resistência mágica ao dano mágico
    {
        // Reduz o dano mágico com base na resistência mágica do alvo e sua inteligência
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);  // Garante que o dano mágico não seja negativo
        return totalMagicDamage;
    }

    public virtual void OnEvasion()
    {

    }
    protected bool AvoidAttacks(CharacterStats _targetStats) //onde contem o evasion stats e o efeito de shocked
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue(); //Calcula a esquiva total considerando a agilidade do alvo e o efeito de choque

        if (isShocked)
            totalEvasion += 20;

        // Gera um número aleatório entre 0 e 100
        // Se o número for menor que a esquiva total, o ataque é evitado
        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CriticalChance()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;

    }

    protected int CalculeCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f; //esta em () para calcular a %
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return health.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.Strength)
            return strength;
        else if (_statType == StatType.Agility)
            return agility;
        else if (_statType == StatType.Intelligence)
            return intelligence;
        else if (_statType == StatType.Vitality)
            return vitality;
        else if (_statType == StatType.Damage)
            return damage;
        else if (_statType == StatType.CritChance)
            return critChance;
        else if (_statType == StatType.CritPower)
            return critPower;
        else if (_statType == StatType.Health)
            return health;
        else if (_statType == StatType.Armor)
            return armor;
        else if (_statType == StatType.Evasion)
            return evasion;
        else if (_statType == StatType.MagicRes)
            return magicResistance;
        else if (_statType == StatType.FireDamage)
            return fireDamage;
        else if (_statType == StatType.IceDamage)
            return iceDamage;
        else if (_statType == StatType.LightiningDamage)
            return lightningDamage;

        return null;
    }
}
