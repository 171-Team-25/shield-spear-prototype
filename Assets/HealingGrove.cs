using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGrove : MonoBehaviour
{
    [SerializeField] int healingPool;
    [SerializeField] int maxHealingPool = 500;
    [SerializeField] int healingPerLoop = 10;
    [SerializeField] int poolReplenishPerLoop = 50;

    [SerializeField] float loopTimeSec = 1f;

    private Dictionary<GameObject, bool> healablesReadyForHealing= new Dictionary<GameObject, bool>();
    private List<GameObject> healablesIn = new List<GameObject>();

    private bool replenishReady = false;
    private bool replenishRunning = false;

    private Renderer matRenderer;
    private Color baseColor;

    private string[] tagsOfHealables = {
        "Offense",
        "Defense"
    };

    private void Start() {
        healingPool = maxHealingPool;
        matRenderer = GetComponent<Renderer>();
        if (matRenderer != null) {
            baseColor = matRenderer.material.color;
        }
    }

    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < tagsOfHealables.Length; i++)
        {
            if (other.CompareTag(tagsOfHealables[i]))
            {
                if (!healablesReadyForHealing.ContainsKey(other.gameObject)) {
                    healablesReadyForHealing[other.gameObject] = true;
                }
                healablesIn.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        for (int i = 0; i < tagsOfHealables.Length; i++)
        {
            if (other.CompareTag(tagsOfHealables[i]))
            {
                healablesReadyForHealing.Remove(other.gameObject);
                healablesIn.Remove(other.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        for (int i = 0; i < tagsOfHealables.Length; i++)
        {
            if (other.CompareTag(tagsOfHealables[i]))
            {
                Health healableHealth = other.gameObject.GetComponent<Health>();
                if (healableHealth == null) {
                    Debug.Log("healable does not have health component");
                    continue;
                }
                if (healingPool <= 0) {
                    continue;
                }
                if (healablesReadyForHealing[other.gameObject] == true && healableHealth.currentHealth < healableHealth.maxHealth && healingPool >= healingPerLoop) {
                    healableHealth.getHealed(healingPerLoop);
                    healablesReadyForHealing[other.gameObject] = false;
                    StartCoroutine(WaitTillHealingReady(other.gameObject));
                    healingPool -= healingPerLoop;
                    UpdateGroveTransparency();
                }
            }
        }
    }

    private void Update() {
        if (healingPool < maxHealingPool && healablesIn.Count <= 0) {
            if (!replenishReady && !replenishRunning) {
                StartCoroutine(WaitTillFillPool());
            } else if (replenishReady) {
                healingPool = Math.Min(maxHealingPool, healingPool + poolReplenishPerLoop);
                replenishReady = false;
                UpdateGroveTransparency();
            }
        }
    }

    private IEnumerator WaitTillHealingReady(GameObject healable) {
        yield return new WaitForSeconds(loopTimeSec);
        if (healablesIn.Contains(healable)) {
            healablesReadyForHealing[healable] = true;
        } else {
            healablesReadyForHealing.Remove(healable);
        }
    }

    private IEnumerator WaitTillFillPool() {
        replenishRunning = true;
        yield return new WaitForSeconds(loopTimeSec);
        replenishReady = true;
        replenishRunning = false;
    }

    private void UpdateGroveTransparency() {
        Color newColor = baseColor;
        newColor.a = (float)healingPool/(float)maxHealingPool;
        matRenderer.material.color = newColor;
    }
}
