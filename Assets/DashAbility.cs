using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashAbility : MonoBehaviour, IAbility
{
    public float dashDistance;

    [SerializeField] private float baseDashDistance = 5f;
    public float dashDurationS;
    public readonly AbilityStats AbilityStats = new AbilityStats
    {
        Cooldown = 3,
        ActivationType = AbilityActivationType.Press,
        UsageType = AbilityUsageType.Instant,
    };

    public readonly AbilityEffect[] Effects =
    {
        new AbilityEffect { EffectType = EffectType.DisableMovement },
        new AbilityEffect { EffectType = EffectType.IgnorePlayerCollision },
    };
    public event EventHandler<AbilityEventArgs> AbilityStarted;
    public event EventHandler<AbilityEventArgs> AbilityEnded;

    private Rigidbody _rigidbody;

    [NonSerialized]
    public AbilityCooldown Cooldown;

    [SerializeField] Text abilityDisplay;
    
    private PlayerStats playerStats;

    public void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        _rigidbody = GetComponent<Rigidbody>();
        Cooldown = gameObject.AddComponent<AbilityCooldown>();
        OnStatChanges();
    }

    public void Activate()
    {
        if (Cooldown.Running)
            return;

        StartCoroutine(Dash());
        if (abilityDisplay.enabled) {
            abilityDisplay.enabled = false;
        }
    }

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

    private IEnumerator Dash()
    {
        var dashTimer = dashDurationS / Time.fixedDeltaTime;
        var dashSpeed = dashDistance / dashTimer / Time.fixedDeltaTime;
        var dashDirection = gameObject.transform.forward;
        var startPosition = gameObject.transform.position;
        
        Cooldown.StartCooldown();
        AbilityStarted?.Invoke(this, new AbilityEventArgs { Ability = this, Owner = gameObject });
        foreach (var effect in Effects)
        {
            effect.StartEffect();
        }
        while (dashTimer >= 0)
        {
            dashTimer--;
            _rigidbody.velocity = dashDirection * dashSpeed;
            yield return new WaitForFixedUpdate();
        }
        foreach (var effect in Effects)
        {
            effect.EndEffect();
        }
        AbilityEnded?.Invoke(this, new AbilityEventArgs { Ability = this, Owner = gameObject });
    }

    void Update() {
        if (!Cooldown.Running && !abilityDisplay.enabled) {
            abilityDisplay.enabled = true;
        }
    }

    public void Release()
    {
        return;
    }

    void OnStatChanges() {
        Cooldown.maxCooldown = AbilityStats.Cooldown / playerStats.Cooldown;
        dashDistance = baseDashDistance * playerStats.Movement;
    }
}
