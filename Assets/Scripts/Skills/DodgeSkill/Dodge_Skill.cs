using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot dodgeUnlockedButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked { get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockedButton;
    public bool dodgeMirageUnlocked { get; private set; }   

    protected override void Start()
    {
        base.Start();

        dodgeUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        dodgeMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockDodgeMirage();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDodge()
    {
        if (dodgeUnlockedButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.Instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockDodgeMirage()
    {
        if(dodgeMirageUnlockedButton.unlocked)
            dodgeMirageUnlocked = true; 
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
    }
}
