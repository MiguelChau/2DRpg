using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stats")]
    public Stat strength; //1 point increace damage by 1 and crit power by 1%
    public Stat agility; //1 point increase evasion by 1% nd crit chance by 1%
    public Stat intelligence; //1 point increas magic damage by 1 and magic resis by 3
    public Stat vitality; //1 point increase health by 3 or 5 points
    [Space]
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; //default value 150%
    [Space]
    [Header("Defensive Sats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    [Space]
    public bool isIgnited; //dmg overtime
    public bool isFrozen; //decrease armor
    public bool isShocked; //reduce accuracy by 20%

    private float ignitedTimer;
    private float frozenTimer;
    private float shockedTimer;

    [SerializeField] private float ailmentsDur = 4;
    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    public int _currentHealth;

    public System.Action onHealthChange;


    protected virtual void Start()
    {
        critPower.DefaultValues(150);
        _currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();

        //example
        //damage.AddModifier(5);
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

        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Burning dmg" + igniteDamage);

            DecreaseHealthBy(igniteDamage);

            if (_currentHealth < 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (AvoidAttacks(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CriticalChance())
        {
            totalDamage = CalculeCriticalDamage(totalDamage);
        }


        totalDamage = ArmorMethod(_targetStats, totalDamage);
        //_targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue();


        totalMagicDamage = TargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0)
            return; //isto serve para que se todos os valores forem 0, durante o while loop para sair desse loop, sem isto a Unity iria freezar e dar overload

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
                Debug.Log("Apply Fire");
                return;
            }

            if (Random.value < .45f && _iceDamage > 0)
            {
                canApplyFrozen = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);
                Debug.Log("Apply ice");
                return;
            }

            if (Random.value < .5f && _lightDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);
                Debug.Log("Apply light");
                return;
            }

        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .15f));

        _targetStats.ApplyElements(canApplyIgnite, canApplyFrozen, canApplyShock);
        //No que for maior é o que é aplicado


    }

    private static int TargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyElements(bool _ignite, bool _frozen, bool _shock)
    {
        if (isIgnited || isFrozen || isShocked)
            return;
        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 2;

            fx.InvokeIgniteFx(ailmentsDur);
        }

        if (_frozen)
        {
            frozenTimer = 2;
            isFrozen = _frozen;

            fx.InvokeFrozenFx(ailmentsDur);
        }

        if (_shock)
        {
            shockedTimer = 2;
            isShocked = _shock;

            fx.InvokeShockFx(ailmentsDur);
        }

    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        Debug.Log(_damage);

        if (_currentHealth < 0)
            Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        _currentHealth -= _damage;

        if(onHealthChange != null)
            onHealthChange();
    }

    protected virtual void Die()
    {
        Debug.Log("Char has died.");
    }

    private int ArmorMethod(CharacterStats _targetStats, int totalDamage) //onde contem o armor stats e values e o efeito de frozen
    {
        if (_targetStats.isFrozen)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool AvoidAttacks(CharacterStats _targetStats) //onde contem o evasion stats e o efeito de shocked
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack Avoided");
            return true;

        }

        return false;
    }

    private bool CriticalChance()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;

    }

    private int CalculeCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f; //esta em () para calcular a %
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
