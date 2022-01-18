using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy AI/Action/Attack")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;

    public float minimumDistanceNeedToAttack = 0;
    public float maximumDistanceNeedToAttack = 3;
}
