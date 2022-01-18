using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AngryState : AIState
{
    public CombatState combatState;
    public float recoveryTime = 1.5f;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        
        if (enemyAIManager.isPerformAction) return combatState;

        if (enemyAIManager.currentRecoveryTime <= 0 && enemyAIManager.isPerformAction == false)
        {
            enemyAIManager.CurrectAction("Angry");
            enemyAnimatorHandler.StopBlendTreeAnimation();
            //enemyAIManager.transform.DOLookAt(enemyAIManager.currentTarget.transform.position, 0.01f);
            int i = Random.Range(0, 2);
            string anim = i == 0 ? "Enemy_Angry_1" : "Enemy_Angry_2";
            enemyAnimatorHandler.PlayTargetAnimation(anim, true);
            enemyAIManager.SetRecoveryTime(recoveryTime);

            return combatState;
        }

        return combatState;
    }
}
