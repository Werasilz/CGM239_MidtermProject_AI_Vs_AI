using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackState : AIState
{
    public CombatState combatState;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        
        if (enemyAIManager.isPerformAction) return combatState;

        Vector3 targetDirection = enemyAIManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.minimumDistanceNeedToAttack)
            {
                return this;
            }
            else if (distanceFromTarget < currentAttack.maximumDistanceNeedToAttack)
            {
                if (viewableAngle <= currentAttack.maximumAttackAngle &&
                    viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if (enemyAIManager.currentRecoveryTime <= 0 && enemyAIManager.isPerformAction == false)
                    {
                        enemyAIManager.CurrectAction("Attacking");
                        enemyAnimatorHandler.StopBlendTreeAnimation();
                        enemyAIManager.transform.DOLookAt(enemyAIManager.currentTarget.transform.position, 0.01f);
                        enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyAIManager.SetRecoveryTime(currentAttack.recoveryTime);
                        currentAttack = null;

                        return combatState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyAIManager);
        }

        return combatState;
    }

    private void GetNewAttack(EnemyAIManager enemyAIManager)
    {
        Vector3 targetDirection = enemyAIManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack &&
                distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                    viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack &&
                distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                    viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null) return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
