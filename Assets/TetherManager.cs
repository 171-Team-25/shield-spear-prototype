using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetherManager : MonoBehaviour
{
    public static TetherManager Instance { get; private set; }
    public GameObject tetherIndicatorPrefab;
    public float pollingRate = 0.5f;
    private Coroutine _pollingRoutine;
    private List<TetherIndicator> _tetherIndicators = new();

    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There should only be one TetherIndicator singleton");
            Destroy(this);
        }
    }
    
    private void OnEnable()
    {
        _pollingRoutine = StartCoroutine(GetPlayers());
    }

    private void OnDisable()
    {
        StopCoroutine(_pollingRoutine);
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
                if (_tetherIndicators.Any(tether => tether.Defense == defense[defenseIndex].transform))
                {
                    defenseIndex++;
                    continue;
                }

                if (_tetherIndicators.Any(tether => tether.Offense == offense[offenseIndex].transform))
                {
                    offenseIndex++;
                    continue;
                }
                var tether = Instantiate(tetherIndicatorPrefab, transform);
                var tetherIndicator = tether.GetComponent<TetherIndicator>();
                offense[offenseIndex].GetComponent<Movement>().Tether = tetherIndicator;
                tetherIndicator.Offense = offense[offenseIndex].transform;
                tetherIndicator.Defense = defense[defenseIndex].transform;
                tetherIndicator.MaxTetherDistance = tetherIndicator.Offense.GetComponent<Movement>().TetherDistance;
                _tetherIndicators.Add(tetherIndicator);
                pairs--;
            }
            yield return new WaitForSeconds(pollingRate);
        }
    }
    
}
