using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : MonoBehaviour
{
    EnemyAIManager enemyAIManager;
    EnemyAnimatorHandler enemyAnimatorHandler;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterColliderBlocker;

    private void Awake()
    {
        enemyAIManager = GetComponent<EnemyAIManager>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterColliderBlocker, true);
    }
}
