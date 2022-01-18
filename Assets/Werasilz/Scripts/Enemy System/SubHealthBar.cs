using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubHealthBar : MonoBehaviour
{
    Image image;
    public bool LookatCamera;
    private float decreaseTime = 0.25f;

    private void Awake()
    {
        image = transform.GetChild(1).GetComponent<Image>();
    }

    private void Update()
    {
        HealthBarLookatCamera();
    }

    void HealthBarLookatCamera()
    {
        if (LookatCamera)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, Camera.main.transform.eulerAngles.y, transform.rotation.z);
        }
    }

    public void SetMaxHealth(float currentLevel, float maxHealth)
    {
        float value = maxHealth / (currentLevel * 100);
        image.fillAmount = value;
    }

    public void SetCurrentHealth(float currentLevel, float currentHealth, bool isSlowDecrease)
    {
        if (!isSlowDecrease)
        {
            float value = currentHealth / (currentLevel * 100);
            image.fillAmount = value;
        }
        else
        {
            float value = currentHealth / (currentLevel * 100);
            DOVirtual.Float(image.fillAmount, value, decreaseTime, SetFillAmount);
        }
    }

    private void SetFillAmount(float value)
    {
        image.fillAmount = value;
    }
}
