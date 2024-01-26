using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerTriggers : EnemyAnimationTrigger
{
    private  Enemy_Bringer enemyBringer => GetComponentInParent<Enemy_Bringer>();

    private void Relocate() => enemyBringer.FindPosition();

    private void MakeInvisible() => enemyBringer.fx.MakeTransparent(true);
    private void MakeVisible() => enemyBringer.fx.MakeTransparent(false);
}
