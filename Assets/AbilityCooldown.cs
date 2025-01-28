using System;
using System.Collections;
using UnityEngine;

public class AbilityCooldown : MonoBehaviour
{
    public float RemainingCooldown => Math.Max(0, _endTime - Time.time);
    public bool Running { get; private set; }
    public float maxCooldown;

    public event EventHandler CooldownStarted;
    public event EventHandler CooldownEnded;

    private Coroutine _cooldownRoutine;
    private float _endTime;

    public void Start()
    {
        CooldownStarted += (s, e) =>
        {
            Running = true;
            Debug.Log("Cooldown started");
        };
        CooldownEnded += (s, e) =>
        {
            Running = false;
            Debug.Log("Cooldown ended");
        };
    }

    public void StartCooldown(bool resetCurrentCooldown = true)
    {
        switch (resetCurrentCooldown)
        {
            case false when _cooldownRoutine != null:
                return;
            case true when _cooldownRoutine != null:
                StopCoroutine(_cooldownRoutine);
                break;
        }

        _cooldownRoutine = StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        CooldownStarted?.Invoke(this, EventArgs.Empty);
        _endTime = Time.time + maxCooldown;
        yield return new WaitForSeconds(maxCooldown);
        CooldownEnded?.Invoke(this, EventArgs.Empty);
    }
}
