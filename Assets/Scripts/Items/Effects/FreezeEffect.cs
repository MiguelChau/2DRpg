using UnityEngine;


[CreateAssetMenu(fileName = "Freeze Effect", menuName = "Data/ItemEffect/Freeze effect")]
public class FreezeEffect : ItemEffect
{
    [SerializeField] private float dur;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats._currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;

        if (!Inventory.Instance.UseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(dur);
        }
    }
}
