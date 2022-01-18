using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpaqueGlow : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;

    public float glowStart = 1.5f;
    public float glowTarget = 0.5f;
    private float glowSpeed;

    private float glowRemaining = 1.5f;
    public bool materialInParent;

    private void Start()
    {
        if (materialInParent)
        {
            skinnedMeshRenderer = GetComponentInParent<SkinnedMeshRenderer>();
        }
        else
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }
    }

    private void Update()
    {
        if (glowRemaining <= glowTarget)
        {
            DOVirtual.Float(glowTarget, glowStart, glowSpeed, StartGlowing);
        }
    }

    public void EnableGlow(float speed)
    {
        glowSpeed = speed;
        DOVirtual.Float(glowStart, glowTarget, glowSpeed, StartGlowing);
    }

    void StartGlowing(float target)
    {
        Material[] mats = skinnedMeshRenderer.materials;
        mats[0].SetFloat("_AlphaThreshold", target);
        glowRemaining = target;
        skinnedMeshRenderer.materials = mats;
    }
}
