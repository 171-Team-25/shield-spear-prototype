using UnityEngine;
using UnityEngine.InputSystem;

public class JoinGameSystem : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    public GameObject offensePrefab;
    public GameObject defensePrefab;
    
    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        var team = (_playerInputManager.playerCount + 1) % 2;
        if (_playerInputManager.playerCount % 2 == 0)
        {
            // Spawn Offense
            offensePrefab = Instantiate(offensePrefab);
        }
        else
        {
            // Spawn Defense
            defensePrefab = Instantiate(defensePrefab);
        }

        Debug.Log("Player " + obj.name + " has joined on team " + team);
    }
}
