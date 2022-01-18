using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : AIState
{
    [Header("AI State")]
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    public RetreatState retreatState;
    public AngryState angryState;

    private bool isRandomMove;
    private float h;
    private float v;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        if (enemyAIManager.isPerformAction)
        {
            enemyAnimatorHandler.StopBlendTreeAnimation();
        }

        if (enemyAIManager.currentTarget == null)
        {
            enemyAnimatorHandler.StopBlendTreeAnimation();

            return patrolState;
        }

        if (enemyAIManager.currentTarget.isDead)
        {
            enemyAnimatorHandler.StopBlendTreeAnimation();

            return patrolState;
        }

        float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);

        if (distanceFromTarget > 1.5f)
        {
            if (enemyAIManager.currentRecoveryTime <= 0 && enemyAIManager.isPerformAction == false)
            {
                HandleRotateTowardTarget(enemyAIManager, enemyAnimatorHandler);
            }
        }

        if (enemyAIManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyAIManager.maximumCombatRange)
        {
            if (isRandomMove == false)
            {
                isRandomMove = true;

                if (Random.Range(0, 2) == 0)
                {
                    enemyAIManager.CurrectAction("Walk Forward/Backward");
                    v = Random.Range(0, 2) == 0 ? 1 : -1;
                    h = 0;
                }
                else
                {
                    enemyAIManager.CurrectAction("Walk Left/Right");
                    v = 0;
                    h = Random.Range(0, 2) == 0 ? 1 : -1;
                }

                StartCoroutine(ClearRandomMove());
            }

            if (distanceFromTarget <= 1.5f)
            {
                v = 0;
                h = 0;

                return RandomCloseAction();
            }

            enemyAnimatorHandler.animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            enemyAnimatorHandler.animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);

            return this;
        }
        else if (distanceFromTarget > enemyAIManager.maximumCombatRange)
        {
            int random = Random.Range(1, 101);

            return RandomRangeAction();
        }
        else
        {
            return this;
        }
    }

    private AIState RandomCloseAction()
    {
        // Retreat 10 %
        // Attack 90 %

        int random = Random.Range(1, 101);

        if (random >= 1 && random < 10)
        {
            return retreatState;
        }
        else
        {
            return attackState;
        }
    }

    private AIState RandomRangeAction()
    {
        // Chase 45 %
        // Retreat 45 %
        // Angry 10 %

        int random = Random.Range(1, 101);
        
        if (random >= 1 && random < 45)
        {
            return chaseState;
        }
        else if (random >= 46 && random < 90)
        {
            return retreatState;
        }
        else
        {
            return angryState;
        }
    }

    private void HandleRotateTowardTarget(EnemyAIManager enemyAIManager, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyAIManager.isPerformAction == false)
        {
            float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);

            if (distanceFromTarget <= enemyAIManager.maximumCombatRange)
            {
                Vector3 direction = enemyAIManager.currentTarget.transform.position - enemyAIManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                enemyAIManager.transform.rotation = Quaternion.Slerp(enemyAIManager.transform.rotation, Quaternion.LookRotation(direction), 5 * Time.deltaTime);
            }
        }
    }

    IEnumerator ClearRandomMove()
    {
        yield return new WaitForSeconds(Random.Range(1, 4));
        isRandomMove = false;
        StopCoroutine(ClearRandomMove());
    }
}