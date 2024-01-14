using UnityEngine;

//gerenciador que mantém uma instancia unica, chamado o padrao de singleton, de cada habilidade no jogo.
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;  //instancia unica do skillmanager - singleton

    public Dash_Skill dash { get; private set; } // ref habilidade de dash
    public Clone_Skill clone { get; private set; } // ref habilidade de clone

    public Sword_Skill sword { get; private set; }  // ref habilidade de atirar sword

    public Blackhole_Skill blackhole { get; private set; } //ref habilidade de blackhole
    public Crystal_Skill1 crystal { get; private set; }
    public ParrySkill parry { get; private set; }

    public Dodge_Skill dodge { get; private set; }

    private void Awake() //este awake grante que apenas exista uma instancia de skillmanager no jogo
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill1>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<Dodge_Skill>();
    }
}
