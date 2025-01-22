using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : Health
{
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            this.gameObject.SetActive(false);
            Invoke("Revive", 2);
        }
    }

    void Revive() {
        this.gameObject.SetActive(true);
        currentHealth = baseHealth;
    }
}
