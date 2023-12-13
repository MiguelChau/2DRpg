using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    //Parametros que controlam o tamanho, velocidade de crescimento, encolhimento do skill e temporizador
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;

    //Paramentros que controlam os estados e açoes do skill
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisappear = true;

    //Parametros para controlar os ataques dos clones e nº dos mesmos
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>(); //lista de alvos dentro de alcande co skill
    private List <GameObject> createdHotKey = new List<GameObject>(); //lista da tecla criada durante o spell

    public bool playerCanExitState { get; private set; } //parametro que dita se o jogador pode ou nao sair do skill

    //Metodo para configurar os parametros inicias do skill
    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackHoleDur)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDur;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisappear = false;
    }

    //Responsavel pelo comportamento do skill a cada frame
    private void Update()
    {
        //Quem reduz o tempo do skill
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        //verifica se o skill atingiu o tempo maximo, liberando assim os clones ou se encerrou o skill
        if(blackHoleTimer <0)
        {
            blackHoleTimer = Mathf.Infinity;

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleSpell();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        //quem controla o crescimento e encolhimento do skill
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack() // Libera os ataques de clonagem. Se houver alvos, inicia os ataques; caso contrário, encerra o skill
    {
        if (targets.Count <= 0)
            return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);    
        }
    }

    private void CloneAttackLogic() //Logica para os ataques dos clones
    {
        // Aguarda o temporizador do ataque de clonagem e, quando liberado, escolhe aleatoriamente um alvo entre os inimigos próximos e cria um clone.
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffSet;

            if (Random.Range(0, 100) > 50)
                xOffSet = 2;
            else
                xOffSet = -2;

            if(SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffSet, 0));
            }
            amountOfAttacks--;

            //A quando atingir o limete de ataques, encerra o spell dentro desse tempo 
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleSpell", 1f);
            }
        }
    }

    //Finaliza a skill
    private void FinishBlackHoleSpell()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }


    private void DestroyHotKeys()// Destroi todas as teclas de atalho criadas durante o skill do mesmo
    {
        if (createdHotKey.Count <= 0)
            return;

        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    // Detecta quando um inimigo entra na zona de influência do skill.
    // Congela o tempo para o inimigo e cria uma tecla de atalho para esse inimigo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    // Detecta quando um inimigo sai da zona de influência do buraco negro.
    // Descongela o tempo para o inimigo.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    //outra  maneira de escrever isto em codigo:
    // private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);



    // Cria uma tecla de atalho para um inimigo específico.
    // Escolhe aleatoriamente uma tecla de atalho disponível e instancia a tecla de atalho no mundo.
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hotkeys");
            return;
        }

        if (!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHoleHotKey newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKey>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
