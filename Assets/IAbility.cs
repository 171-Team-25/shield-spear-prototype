using System;
using UnityEngine;

public interface IAbility
{
    public event EventHandler<AbilityEventArgs> AbilityStarted;
    public event EventHandler<AbilityEventArgs> AbilityEnded;
    public void Activate();
    public AbilityActivationType GetActivationType();
    public AbilityUsageType GetUsageType();
    public AbilityEffect[] GetEffects();
}

public class AbilityEventArgs : EventArgs
{
    public GameObject Owner;
    public IAbility Ability;
}

public enum AbilityActivationType
{
    None,
    Press,
    Hold,
    Release,
}

public enum AbilityUsageType
{
    Instant,
    Channeled,
}

public struct AbilityStats
{
    public float Cooldown;
    public AbilityActivationType ActivationType;
    public AbilityUsageType UsageType;
}
