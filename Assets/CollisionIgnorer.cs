using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    public void Start()
    {
        if (TryGetComponent<CurrentTeam>(out var currentTeam))
        {
            OnTeamChanged(currentTeam.Team, currentTeam.Team == 1 ? 2 : 1);
        }
        else
        {
            Debug.LogWarning("Collision Ignorer: There is no CurrentTeam attached to this object for.");
        }
    }

    private void OnTeamChanged(int team, int previousTeam)
    {
        // Only Change Layer if a player
        Debug.Log("Collision Ignorer: Received Team Changed Event.");
        if (gameObject.CompareTag("Defense") || gameObject.CompareTag("Offense"))
        {
            var layer = LayerMask.NameToLayer($"Team{team} Player");
            var oldLayer = LayerMask.GetMask($"Team{previousTeam} Player");
            gameObject.layer = layer;
            var collider = gameObject.GetComponent<Collider>();
            var excludedLayers = collider.excludeLayers;
            //collider.excludeLayers &= ~oldLayer;
            collider.excludeLayers |= 1 << layer;
        }
    }
}
