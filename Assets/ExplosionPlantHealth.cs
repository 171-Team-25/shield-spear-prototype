using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ExplosionPlantHealth : Health
{
    private Collider plantCollider;
    void Start()
    {
        plantCollider = GetComponent<Collider>();
        currentHealth = maxHealth;
        if (gameObject.tag == "Defense") {
            matRenderer = transform.Find("BodySize").gameObject.GetComponent<Renderer>();
        } else {
            matRenderer = GetComponent<Renderer>();
        }
        if (matRenderer != null) {
            baseColor = matRenderer.material.color;
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            if (isDead) {
                return;
            }
            Die();
        }
    }

    new void Die() {
        isDead = true;
        Invoke("Disappear", 0.25f);
        Explode();
        Invoke("Revive", timeDeadS);
        Invoke("Shrink", timeDeadS);
    }

    void Explode() {
        transform.localScale = new UnityEngine.Vector3(10, 0.5f, 10);
    }

    void Disappear() {
        if (matRenderer != null) {
            matRenderer.enabled = false;
        }
    }

    void Shrink() {
        transform.localScale = new UnityEngine.Vector3(1,1,1);
    }

    void Revive() {
        isDead = false;
        matRenderer.enabled = true;
        currentHealth = baseHealth;
    }
}
