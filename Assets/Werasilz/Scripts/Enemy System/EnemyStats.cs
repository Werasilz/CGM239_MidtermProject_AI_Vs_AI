using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health Stats")]
    public int currentHealth;

    [Header("Damage")]
    public int baseDamage;

    [Header("Enemy Status")]
    public bool isDead;

    [Header("Effect")]
    public OpaqueGlow[] opaqueGlow;
    public GameObject hitEffect;

    EnemyAnimatorHandler enemyAnimatorHandler;
    GameObject enemyUI;
    EnemyAIManager enemyAIManager;

    private void Awake()
    {
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        enemyAIManager = GetComponent<EnemyAIManager>();
    }

    private void Update()
    {
        if (isDead == false && currentHealth <= 0)
        {
            EnemyDead();
            enemyAIManager.currentState = null;
        }
    }

    public void TakeDamage(int damage, GameObject effect)
    {
        if (isDead) return;

        enemyAIManager.currentState = enemyAIManager.combatState;
        enemyAnimatorHandler.CloseWeaponDamageCollider();

        currentHealth = currentHealth - damage;

        enemyAIManager.CurrectAction("Hurt");
        enemyAIManager.SetRecoveryTime(0.5f);
        enemyAnimatorHandler.ForceExitAnimation();
        enemyAnimatorHandler.PlayTargetAnimation("Enemy_Hurt_1", true);

        for (int i = 0; i < opaqueGlow.Length; i++)
        {
            opaqueGlow[i].EnableGlow(0.25f);
        }

        GameObject hitEffect = Instantiate(effect, this.transform.position + Vector3.up, this.transform.rotation);
    }

    private void EnemyDead()
    {
        currentHealth = 0;
        enemyAIManager.CurrectAction("Dead");
        enemyAIManager.SetRecoveryTime(0.5f);
        enemyAnimatorHandler.ForceExitAnimation();
        enemyAnimatorHandler.PlayTargetAnimation("Enemy_Dead_1", false);
        isDead = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }
}
