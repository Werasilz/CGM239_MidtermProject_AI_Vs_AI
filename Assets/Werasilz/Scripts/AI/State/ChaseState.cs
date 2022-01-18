using UnityEngine;

public class ChaseState : AIState
{
    public PatrolState patrolState;
    public CombatState combatState;

    public float limitChaseDistance = 20;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyAIManager.isPerformAction)
        {
            enemyAnimatorHandler.StopBlendTreeAnimation();

            return this;
        }

        Vector3 targetDirection = enemyAIManager.currentTarget.transform.position - enemyAIManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyAIManager.currentTarget.transform.position, enemyAIManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, enemyAIManager.transform.forward);

        if (distanceFromTarget > enemyAIManager.maximumAttackRange)
        {
            enemyAIManager.CurrectAction("Chasing");

            enemyAnimatorHandler.animator.SetFloat("Vertical", 1.5f, 0.1f, Time.deltaTime);
            enemyAnimatorHandler.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
        }

        if (distanceFromTarget > limitChaseDistance)
        {
            enemyAIManager.CurrectAction("Lost Target");
            enemyAIManager.currentTarget = null;

            return patrolState;
        }

        if (enemyAIManager.currentTarget.isDead)
        {
            enemyAIManager.currentTarget = null;
            return patrolState;
        }

        if (enemyAIManager.currentRecoveryTime <= 0 && enemyAIManager.isPerformAction == false)
        {
            HandleRotateTowardTarget(enemyAIManager);
            enemyAIManager.agent.transform.localPosition = Vector3.zero;
            enemyAIManager.agent.transform.localRotation = Quaternion.identity;
        }

        if (distanceFromTarget <= enemyAIManager.maximumAttackRange)
        {
            return combatState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardTarget(EnemyAIManager enemyAIManager)
    {
        if (enemyAIManager.isPerformAction)
        {
            Vector3 direction = enemyAIManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyAIManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyAIManager.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyAIManager.agent.desiredVelocity);
            Vector3 targetVelecity = enemyAIManager.rb.velocity;

            enemyAIManager.agent.enabled = true;
            enemyAIManager.agent.SetDestination(enemyAIManager.currentTarget.transform.position);
            enemyAIManager.rb.velocity = targetVelecity;
            enemyAIManager.transform.rotation = Quaternion.Slerp(enemyAIManager.transform.rotation, enemyAIManager.agent.transform.rotation, enemyAIManager.rotationSpeed / Time.deltaTime);
        }
    }
}