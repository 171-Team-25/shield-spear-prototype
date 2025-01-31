using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealth : Health
{
    [SerializeField] Text abilityDisplay;

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            abilityDisplay.enabled = false;
            this.gameObject.SetActive(false);
            Invoke("Revive", 2);
        }
    }

    void Revive()
    {
        abilityDisplay.enabled = true;
        this.gameObject.SetActive(true);
        currentHealth = baseHealth;
    }
}
