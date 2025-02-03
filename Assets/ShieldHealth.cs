using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealth : Health
{
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            this.isDead = true;
            Invoke("Revive", 2);
        }
    }

    void Revive()
    {
        this.isDead = false;
        currentHealth = baseHealth;
    }
}
