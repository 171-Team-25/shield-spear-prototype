using System;
using UnityEngine;

public class CurrentTeam : MonoBehaviour
{
    [SerializeField] private int currentTeam;
    public int Team
    {
        get => currentTeam;
        set
        {
            if (value == currentTeam) return;
            currentTeam = value;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        Team = currentTeam;
    }
}
