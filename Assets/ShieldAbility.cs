using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldAbility : MonoBehaviour, IAbility
{
    [SerializeField] Text abilityDisplay;
    [NonSerialized] public AbilityCooldown Cooldown;
    public event EventHandler<AbilityEventArgs> AbilityStarted;
    public event EventHandler<AbilityEventArgs> AbilityEnded;

    public readonly AbilityEffect[] Effects = {};

    public readonly AbilityStats AbilityStats = new AbilityStats
    {
        Cooldown = 0.1f,
        ActivationType = AbilityActivationType.Hold,
        UsageType = AbilityUsageType.Channeled,
    };
    
    public AbilityActivationType GetActivationType()
    {
        return AbilityStats.ActivationType;
    }

    public AbilityUsageType GetUsageType()
    {
        return AbilityStats.UsageType;
    }

    public AbilityEffect[] GetEffects()
    {
        return Effects;
    }

    [SerializeField] private GameObject shieldPrefab;
    private GameObject shieldObject;
    private Shield shield;
    private ShieldHealth shieldHealth;
    private PlayerStats playerStats;
    public void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        Cooldown = gameObject.AddComponent<AbilityCooldown>();
        Cooldown.maxCooldown = AbilityStats.Cooldown;
        shieldObject = Instantiate(shieldPrefab);
        shield = shieldObject.transform.Find("ShieldHitbox").GetComponent<Shield>();
        shieldHealth = shieldObject.transform.Find("ShieldHitbox").GetComponent<ShieldHealth>();
        shieldObject.transform.SetParent(this.transform);
        shieldObject.transform.localPosition = Vector3.zero;
        OnStatChanges();
    }

    public void Activate()
    {
        if (Cooldown.Running)
            return;

        ShieldUp();
    }

    public void Release()
    {
        ShieldDown();
    }

    public void ShieldUp() {
        if (!shieldHealth.isDead) {
            Cooldown.StartCooldown();
            AbilityStarted?.Invoke(this, new AbilityEventArgs { Ability = this, Owner = gameObject });
            shield.shieldUp = true;
            abilityDisplay.enabled = false;
            shield.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void ShieldDown() {
        AbilityEnded?.Invoke(this, new AbilityEventArgs { Ability = this, Owner = gameObject });
        shield.shieldUp = false;
        shield.GetComponent<BoxCollider>().enabled = false;
    }

    void Update() {
        if (abilityDisplay.enabled == true && shieldHealth.isDead) {
            AbilityEnded?.Invoke(this, new AbilityEventArgs { Ability = this, Owner = gameObject });
            abilityDisplay.enabled = false;
            ShieldDown();
        }
        if (abilityDisplay.enabled == false && !shieldHealth.isDead && !Cooldown.Running) {
            abilityDisplay.enabled = true;
        }
    }

    void OnStatChanges() {
        shield.AreaSizeUpdated(playerStats.AreaSize);
        shieldHealth.HealthUpdated(playerStats.Health);
    }
}
