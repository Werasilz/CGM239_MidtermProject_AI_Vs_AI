using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AIState
{
    [Header("AI State")]
    public ChaseState chaseState;

    [Header("Detection Setting")]
    public LayerMask detectionLayer;

    [Header("Waypoint Setting")]
    public Transform[] waypoints;
    public int currentWayPoint;
    public float stoppingDistance;
    public int minRandomWait = 2;
    public int maxRandomWait = 5;
    private bool isRandomWaypoint;

    public override AIState Action(EnemyAIManager enemyAIManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyAIManager.currentTarget == null)
        {
            enemyAIManager.CurrectAction("Searching");
            HandleRotateTowardTarget(enemyAIManager, enemyAnimatorHandler);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyAIManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            EnemyStats target = colliders[i].transform.GetComponent<EnemyStats>();

            if (target != null)
            {
                Vector3 targetDirection = target.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyAIManager.minimumDetectionAngle && viewableAngle < enemyAIManager.maximumDetectionAngle)
                {
                    enemyAIManager.CurrectAction("Found Target");
                    enemyAIManager.currentTarget = target;
                }
            }
        }

        if (enemyAIManager.currentTarget != null)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardTarget(EnemyAIManager enemyAIManager, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyAIManager.isPerformAction == false)
        {
            StartRandomWaypoint();
            float distanceFromWaypoint = Vector3.Distance(waypoints[currentWayPoint].position, enemyAIManager.transform.position);

            if (distanceFromWaypoint > stoppingDistance)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                enemyAnimatorHandler.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

                Vector3 direction = waypoints[currentWayPoint].position - enemyAIManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                enemyAIManager.transform.rotation = Quaternion.Slerp(enemyAIManager.transform.rotation, Quaternion.LookRotation(direction), (enemyAIManager.rotationSpeed / 5) * Time.deltaTime);
            }
            else
            {
                enemyAnimatorHandler.StopBlendTreeAnimation();
                StartRandomWaypoint();
            }
        }
    }

    private void StartRandomWaypoint()
    {
        if (isRandomWaypoint == false)
        {
            isRandomWaypoint = true;
            StartCoroutine(RandomWaypoint());
        }
    }

    IEnumerator RandomWaypoint()
    {
        yield return new WaitForSeconds(Random.Range(minRandomWait, maxRandomWait + 1));
        currentWayPoint = Random.Range(0, waypoints.Length);
        isRandomWaypoint = false;
        StopCoroutine(RandomWaypoint());
    }
}