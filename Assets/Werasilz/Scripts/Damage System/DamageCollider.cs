using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyStats thisEnemy = this.GetComponentInParent<EnemyStats>();

            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                if (enemyStats.currentHealth > 0)
                {
                    if (thisEnemy != null)
                    {
                        enemyStats.TakeDamage(thisEnemy.baseDamage, thisEnemy.hitEffect);
                    }
                }
            }
        }
    }
}
