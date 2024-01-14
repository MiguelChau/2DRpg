using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//este script gerencia a criaçao de clones, levando em consideraçao as condiçoes e opçoes definidas no Editor. 
public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDur;
    [SerializeField] private float attackMultiplier;

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multicloneAttackMultiplier;
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool duplicateClone;

    [Header("Cystal instead of Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadCloneUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadClone);
    }

    #region Unlock SkillTree

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockCrystalInsteadClone();
        UnlockMultiClone();
    }
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if(aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if(multipleUnlockButton.unlocked)
        {
            duplicateClone = true;
            attackMultiplier = multicloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadClone()
    {
        if (crystalInsteadCloneUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }
    #endregion
    public void CreateClone(Transform _clonePosition, Vector3 _offset) //metodo para criar um clone na posiçao especificada com um deslocamento
    {
        if (crystalInsteadOfClone) // verifica se a opçao esta ativa. se sim chama o cristal em vez de criar um clone
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab); //instancia um novo clone a partir do prefab

        newClone.GetComponent<Clone_SkillController>(). //obtem o componente do controller e chama o seu metodo de setup para configurar o comportamento
            SetupClone(_clonePosition, cloneDur, canAttack, _offset, FindClosestEnemy(newClone.transform), duplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.2f);
        CreateClone(_transform, _offset);
    }

  
}
