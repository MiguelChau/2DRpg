using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

//Este script cria as habilidades dos cristais com diferentes comportamentos.
public class Crystal_Skill1 : Skill
{
    [SerializeField] private float crystalDuration; //duração do cristal
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal; //referencia para o atual crystal

    [Header("Crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalUnlockedButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot crystalMirageUnlockedButton;
    [SerializeField] private bool cloneInsteadOfCrystal; //referencia para alterar o clone para cristal

    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalExplosiveUnlockedButton;
    [SerializeField] private float explosiveCD;
    [SerializeField] private bool canExplode; //explosao do cristal


    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalMoveExplosiveUnlockedButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;


    [Header("Multi stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalMultiDestroUnlockedButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWondow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        crystalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        crystalMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        crystalExplosiveUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosive);
        crystalMoveExplosiveUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoving);
        crystalMultiDestroUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    #region UnlockCrystalSkills

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalExplosive();
        UnlockCrystalMirage();
        UnlockCrystalMoving();
        UnlockMultiCrystal();
    }
    private void UnlockCrystal()
    {
        if (crystalUnlockedButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if(crystalMirageUnlockedButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockCrystalExplosive()
    {
        if(crystalExplosiveUnlockedButton.unlocked)
        {
            canExplode = true;
            cooldown = explosiveCD;

        }
    }

    private void UnlockCrystalMoving()
    {
        if(crystalMoveExplosiveUnlockedButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultiCrystal()
    {
        if (crystalMultiDestroUnlockedButton.unlocked)
            canUseMultiStacks = true;
    }

    #endregion
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) //verifica se é possivel usar varios cristais
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();

        }
        else //cria um novo cristal se nao houver um atual ou relaiza troca de posiçao ou criação de clone conforme especificado
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos; //troca da posiçao do jogador com a posiçao do cristal atual

            if (cloneInsteadOfCrystal) //verifica se a opçao é criar um clone em vez de um cristal. Se for verdadeiro, cria um clone e destroi o cristal atual.
                //se for falso, finaliza o cristal atual chamando o finishcrystal.
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal() //instancia um novo cristal, configurando o seu comportamento com base nas variaveis
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    //metodo para escolher random um inimigo para o cristal atuar
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy(); 

    private bool CanUseMultiCrystal() //responsavel por verificar se é possivel usar multiplos cristais
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWondow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0) //ffaz refil da lista de cristais quando preciso
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }


                return true;

            }
        }


        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
