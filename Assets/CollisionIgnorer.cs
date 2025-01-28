using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    private LayerMask _defaultLayerMask;
    private int _defaultLayer;
    private Collider[] _colliders;

    public void Start()
    {
        _colliders = GetComponents<Collider>();
        if (TryGetComponent<CurrentTeam>(out var currentTeam))
        {
            OnTeamChanged(currentTeam.Team, currentTeam.Team == 1 ? 2 : 1);
        }
        else
        {
            Debug.LogWarning(
                "Collision Ignorer: There is no CurrentTeam attached to this object for."
            );
        }
    }

    private void OnTeamChanged(int team, int previousTeam)
    {
        // Only Change Layer if a player
        Debug.Log("Collision Ignorer: Received Team Changed Event.");
        if (gameObject.CompareTag("Defense") || gameObject.CompareTag("Offense"))
        {
            var layer = LayerMask.NameToLayer($"Team{team} Player");
            _defaultLayerMask = LayerMask.GetMask(LayerMask.LayerToName(layer));
            _defaultLayer = layer;
            gameObject.layer = layer;
            foreach (var collider in _colliders)
            {
                collider.excludeLayers |= 1 << layer;
            }
        }
    }

    public void IgnoreAllPlayers()
    {
        foreach (var collider in _colliders)
        {
            collider.excludeLayers |= 1 << LayerMask.NameToLayer($"Team1 Player");
            collider.excludeLayers |= 1 << LayerMask.NameToLayer($"Team2 Player");
        }
    }

    public void ResetToDefault()
    {
        gameObject.layer = _defaultLayer;
        foreach (var collider in _colliders)
        {
            collider.excludeLayers = _defaultLayerMask;
        }
    }
}
