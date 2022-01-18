using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : AIState
{
    public CombatState combatState;
    public float recoveryTime = 3f;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyAIManager.isPerformAction) return combatState;

        Vector3 targetDirection = enemyAIManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyAIManager.currentRecoveryTime <= 0 && enemyAIManager.isPerformAction == false)
        {
            enemyAIManager.CurrectAction("Retreat");
            enemyAnimatorHandler.StopBlendTreeAnimation();
            enemyAnimatorHandler.PlayTargetAnimation("Roll_Back", true);
            enemyAIManager.SetRecoveryTime(recoveryTime);

            return combatState;
        }

        return combatState;
    }
}
