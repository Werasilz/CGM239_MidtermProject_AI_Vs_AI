using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    EnemyLocomotion enemyLocomotion;
    EnemyAnimatorHandler enemyAnimatorHandler;
    EnemyStats enemyStats;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody rb;

    public float detectionRadius;
    public float minimumDetectionAngle = -50;
    public float maximumDetectionAngle = 50;
    public float maximumAttackRange = 3f;
    public float maximumCombatRange = 5f;
    public float rotationSpeed = 25;

    [Header("AI Status")]
    public bool isPerformAction;
    public bool isInteracting;
    public float currentRecoveryTime = 0;

    [Header("AI State")]
    public AIState currentState;
    public string currentAction;

    [Header("All State")]
    public PatrolState patrolState;
    public ChaseState chaseState;
    public CombatState combatState;
    public AttackState attackState;
    public RetreatState retreatState;
    public AngryState angryState;

    [Header("AI Target")]
    public EnemyStats currentTarget;

    private void Awake()
    {
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        enemyStats = GetComponent<EnemyStats>();

        agent = GetComponentInChildren<NavMeshAgent>();
        agent.enabled = false;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            AIState nextState = currentState.Action(this, enemyStats, enemyAnimatorHandler);

            if (nextState != null)
            {
                SwitchNextState(nextState);
            }
        }
    }

    private void SwitchNextState(AIState state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformAction = false;
                currentRecoveryTime = 0;
            }
        }
    }

    public void SetRecoveryTime(float time)
    {
        isPerformAction = true;
        currentRecoveryTime = time;
    }

    public void CurrectAction(string action)
    {
        currentAction = action;
    }
}
