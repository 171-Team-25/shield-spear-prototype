using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField]private Camera _cam;

    public void UpdateHealthBar(float maxHealth, float currentHealth) {
        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    void Update() {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
