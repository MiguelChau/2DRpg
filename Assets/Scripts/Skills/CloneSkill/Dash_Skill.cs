using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Clone on Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;

    [Header("Clone on Arrival")]
    public bool cloneOnArrivalDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalDashUnlockButton;

    public override void UseSkill()
    {
        base.UseSkill();

    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        cloneOnArrivalDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDashArrival);
        
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneDash();
        UnlockCloneDashArrival();
    }

    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
            dashUnlocked = true;      
    }

    private void UnlockCloneDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneDashArrival()
    {
        if(cloneOnArrivalDashUnlockButton.unlocked)
            cloneOnArrivalDashUnlocked = true;
    }


    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
          SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrivalDash()
    {
        if (cloneOnArrivalDashUnlocked)
           SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

}
