using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Image image;
    SubHealthBar subHealthBar;
    public bool LookatCamera;

    private void Awake()
    {
        image = transform.GetChild(1).GetComponent<Image>();
        subHealthBar = GetComponentInParent<SubHealthBar>();
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
        subHealthBar.SetMaxHealth(currentLevel, maxHealth);
    }

    public void SetCurrentHealth(float currentLevel, float currentHealth, bool isSlowDecrease)
    {
        float value = currentHealth / (currentLevel * 100);
        image.fillAmount = value;
        subHealthBar.SetCurrentHealth(currentLevel, currentHealth, isSlowDecrease);
    }
}
