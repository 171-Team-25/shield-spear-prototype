using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitySystem : MonoBehaviour
{
    public int maxNumberOfAbilities = 3;
    public List<IAbility> Abilities;
    private PlayerInput _playerInput;

    public void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (!_playerInput)
        {
            Debug.LogError("Ability System: No player input found");
            return;
        }
        Abilities = GetComponents<IAbility>().ToList();
        Abilities = Abilities.GetRange(0, Math.Min(maxNumberOfAbilities, Abilities.Count));
        _playerInput.actions["Ability1"].performed += OnperformedAbility1;
        _playerInput.actions["Ability2"].performed += OnperformedAbility2;
        _playerInput.actions["Ability3"].performed += OnperformedAbility3;
        _playerInput.actions["Ability1"].canceled += OncanceledAbility1;
        _playerInput.actions["Ability2"].canceled += OncanceledAbility2;
        _playerInput.actions["Ability3"].canceled += OncanceledAbility3;
        foreach (var ability in Abilities)
        {
            ability.AbilityStarted += OnAbilityStarted;
            ability.AbilityEnded += OnAbilityEnded;
        }
    }

    private void OnperformedAbility1(InputAction.CallbackContext obj)
    {
        UseAbility(0);
    }

    private void OnperformedAbility2(InputAction.CallbackContext obj)
    {
        UseAbility(1);
    }

    private void OnperformedAbility3(InputAction.CallbackContext obj)
    {
        UseAbility(2);
    }
    
    private void OncanceledAbility1(InputAction.CallbackContext obj)
    {
        releaseAbility(0);
    }
    private void OncanceledAbility2(InputAction.CallbackContext obj)
    {
        releaseAbility(1);
    }
    private void OncanceledAbility3(InputAction.CallbackContext obj)
    {
        releaseAbility(2);
    }

    public bool UseAbility(int abilityIndex)
    {
        if (GetComponent<Health>().isDead) {
            return false;
        }
        if (Abilities.Count <= abilityIndex)
            return false;
        var ability = Abilities[abilityIndex];
        ability.AbilityStarted += OnAbilityStarted;
        ability.AbilityEnded += OnAbilityEnded;
        var effects = ability.GetEffects();
        foreach (var effect in effects)
        {
            effect.EffectStarted += OnEffectStarted;
            effect.EffectEnded += OnEffectEnded;
        }        
        ability.Activate();
        return true;
    }

    public bool releaseAbility(int abilityIndex) {
        if (GetComponent<Health>().isDead) {
            return false;
        }
        if (Abilities.Count <= abilityIndex)
            return false;
        var ability = Abilities[abilityIndex];
        ability.AbilityStarted += OnAbilityStarted;
        ability.AbilityEnded += OnAbilityEnded;
        var effects = ability.GetEffects();
        foreach (var effect in effects)
        {
            effect.EffectStarted += OnEffectStarted;
            effect.EffectEnded += OnEffectEnded;
        }        
        ability.Release();
        return true;
    }

    private void OnAbilityStarted(object sender, AbilityEventArgs e)
    {
        e.Ability.AbilityStarted -= OnAbilityStarted;
    }

    private void OnAbilityEnded(object sender, AbilityEventArgs e)
    {
        e.Ability.AbilityEnded -= OnAbilityEnded;
    }

    private void OnEffectStarted(object sender, EffectEventArgs e)
    {
        e.AbilityEffect.EffectStarted -= OnEffectStarted;
        switch (e.EffectType)
        {
            case EffectType.DisableMovement:
                if (TryGetComponent<Movement>(out var movement))
                {
                    movement.enabled = false;
                }
                else if (TryGetComponent<DefenseMovement>(out var defenseMovement))
                {
                    defenseMovement.enabled = false;
                }
                break;
            case EffectType.IgnorePlayerCollision:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnEffectEnded(object sender, EffectEventArgs e)
    {
        e.AbilityEffect.EffectEnded -= OnEffectEnded;
        switch (e.EffectType)
        {
            case EffectType.DisableMovement:
                if (TryGetComponent<Movement>(out var movement))
                {
                    movement.enabled = true;
                }
                else if (TryGetComponent<DefenseMovement>(out var defenseMovement))
                {
                    defenseMovement.enabled = true;
                }
                break;
            case EffectType.IgnorePlayerCollision:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool UseAbility(IAbility ability)
    {
        return Abilities.Contains(ability) && UseAbility(Abilities.IndexOf(ability));
    }

    public void AddEffect(AbilityEffect effect)
    {
        effect.EffectStarted += OnEffectStarted;
        effect.EffectEnded += OnEffectEnded;
    }
}
