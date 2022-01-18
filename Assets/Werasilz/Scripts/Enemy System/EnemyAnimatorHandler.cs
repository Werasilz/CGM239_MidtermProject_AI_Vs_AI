using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    EnemyAIManager enemyAIManager;
    DamageCollider weaponDamageCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAIManager = GetComponentInParent<EnemyAIManager>();

        weaponDamageCollider = GetComponentInChildren<DamageCollider>();

    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void ForceExitAnimation()
    {
        animator.SetBool("forceExit", true);
    }

    public void StopBlendTreeAnimation()
    {
        animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyAIManager.rb.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyAIManager.rb.velocity = velocity;
    }

    public void OpenWeaponDamageCollider()
    {
        weaponDamageCollider.EnableDamageCollider();
    }

    public void CloseWeaponDamageCollider()
    {
        weaponDamageCollider.DisableDamageCollider();
    }
}
