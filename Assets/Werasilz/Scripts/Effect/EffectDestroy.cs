using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    float timer;
    public float destroyTimer = 3;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= destroyTimer)
        {
            Destroy(gameObject);
        }
    }
}
