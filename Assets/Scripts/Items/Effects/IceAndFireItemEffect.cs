using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/ItemEffect/IceAndFire")]
public class IceAndFireItemEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePRefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respawnPos)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if(thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePRefab, _respawnPos.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 10); ;
        }
       
    }
}
