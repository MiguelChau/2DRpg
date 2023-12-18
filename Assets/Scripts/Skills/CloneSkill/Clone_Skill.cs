using System.Collections;
using UnityEngine;

//este script gerencia a criaçao de clones, levando em consideraçao as condiçoes e opçoes definidas no Editor. 
public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDur;
    [Space]
    [SerializeField] private bool canAttack;
    [Space]
    [SerializeField] private bool createCloneOnDashStart; //Dash mirage
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone Duplicate")]
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool duplicateClone;

    [Header("Cystal instead of Clone")]
    public bool crystalInsteadOfClone;


    public void CreateClone(Transform _clonePosition, Vector3 _offset) //metodo para criar um clone na posiçao especificada com um deslocamento
    {
        if (crystalInsteadOfClone) // verifica se a opçao esta ativa. se sim chama o cristal em vez de criar um clone
        {
            SkillManager.instance.crystal.CreateCrystal();           
            return;
        }

        GameObject newClone = Instantiate(clonePrefab); //instancia um novo clone a partir do prefab

        newClone.GetComponent<Clone_SkillController>(). //obtem o componente do controller e chama o seu metodo de setup para configurar o comportamento
            SetupClone(_clonePosition, cloneDur, canAttack, _offset, FindClosestEnemy(newClone.transform), duplicateClone, chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.2f);
        CreateClone(_transform, _offset);
    }
}
