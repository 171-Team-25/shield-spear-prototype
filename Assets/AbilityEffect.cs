using System;

public class AbilityEffect
{
    public event EventHandler<EffectEventArgs> EffectStarted;
    public event EventHandler<EffectEventArgs> EffectEnded;
    public EffectType EffectType;

    public void StartEffect() =>
        EffectStarted?.Invoke(
            this,
            new EffectEventArgs { AbilityEffect = this, EffectType = EffectType }
        );

    public void EndEffect() =>
        EffectEnded?.Invoke(
            this,
            new EffectEventArgs { AbilityEffect = this, EffectType = EffectType }
        );
}

public class EffectEventArgs : EventArgs
{
    public AbilityEffect AbilityEffect;
    public EffectType EffectType;
}

public enum EffectType
{
    DisableMovement,
    IgnorePlayerCollision,
}
