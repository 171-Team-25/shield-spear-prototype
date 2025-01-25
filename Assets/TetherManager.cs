using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TetherManager : MonoBehaviour
{
    List<TetherIndicator> tetherIndicators = new();
    public GameObject tetherIndicatorPrefab;
    public float pollingRate = 0.5f;
    private Coroutine pollingRoutine;
    
    private void OnEnable()
    {
        pollingRoutine = StartCoroutine(GetPlayers());
    }

    private void OnDisable()
    {
        StopCoroutine(pollingRoutine);
    }

    private IEnumerator GetPlayers()
    {
        while (true)
        {
            var defense = GameObject.FindGameObjectsWithTag("Defense");
            var offense = GameObject.FindGameObjectsWithTag("Offense");
            var pairs = defense.Length + offense.Length / 2;
            if (defense.Length == 0 || offense.Length == 0)
                yield return new WaitForSeconds(pollingRate);
            var defenseIndex = 0;
            var offenseIndex = 0;
            while (pairs > 0 && defenseIndex < defense.Length && offenseIndex < offense.Length)
            {
                if (tetherIndicators.Any(tether => tether.Defense == defense[defenseIndex].transform))
                {
                    defenseIndex++;
                    continue;
                }

                if (tetherIndicators.Any(tether => tether.Offense == offense[offenseIndex].transform))
                {
                    offenseIndex++;
                    continue;
                }
                var tether = Instantiate(tetherIndicatorPrefab, this.transform);
                var tetherIndicator = tether.GetComponent<TetherIndicator>();
                tetherIndicator.Offense = offense[offenseIndex].transform;
                tetherIndicator.Defense = defense[defenseIndex].transform;
                tetherIndicators.Add(tetherIndicator);
                pairs--;
            }
            yield return new WaitForSeconds(pollingRate);
        }
    }
    
}
