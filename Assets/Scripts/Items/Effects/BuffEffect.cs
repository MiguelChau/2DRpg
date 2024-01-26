using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/ItemEffect/Buff")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDur;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDur, stats.GetStat(buffType));

    }


}
