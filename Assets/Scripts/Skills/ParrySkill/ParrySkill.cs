using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("LifeSteal on Parry")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry Mirage")]
    [SerializeField] private UI_SkillTreeSlot mirageParryUnlockButton;
    public bool mirageParryUnlocked { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthAmount);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryLifeSteal);
        mirageParryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMirageParry);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryLifeSteal();
        UnlockMirageParry();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryLifeSteal()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockMirageParry()
    {
        if (mirageParryUnlockButton.unlocked)
            mirageParryUnlocked = true;
    }

    public void DoMirageOnParry(Transform _respawnTransform)
    {
        if (mirageParryUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
