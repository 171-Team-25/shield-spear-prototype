using UnityEngine;
using UnityEngine.InputSystem;

public class JoinGameSystem : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private PlayerInputMap _playerInputMap;
    // Player Prefabs
    public GameObject offensePrefab;
    public GameObject defensePrefab;
    public bool fillTeamsInOrder = true;
    private void Start()
    {
        
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
        _playerInputMap = new PlayerInputMap();
        _playerInputMap.Enable();
        _playerInputMap.Default.Join.started += OnJoinInput;
    }

    private void OnJoinInput(InputAction.CallbackContext ctx)
    {
        // Alternate player prefabs to spawn to create balanced teams
        if (_playerInputManager.playerCount % 2 == 0)
        {
            _playerInputManager.playerPrefab = offensePrefab;
        }
        else
        {
            _playerInputManager.playerPrefab = defensePrefab;
        }

        _playerInputManager.JoinPlayer(pairWithDevice: ctx.control.device);
    }
    
    private void OnPlayerJoined(PlayerInput obj)
    {
        var playerCount = _playerInputManager.playerCount;
        var teamSize = _playerInputManager.maxPlayerCount / 2;
        var team = fillTeamsInOrder ? (playerCount > teamSize ? 2 : 1) : (playerCount + 1) % 2 + 1;
        Debug.Log("Player " + _playerInputManager.playerCount + " has joined on team " + team);
        if (obj.gameObject.TryGetComponent<CurrentTeam>(out var currentTeam))
        {
            currentTeam.Team = team;
        }
        else
        {
            Debug.LogError("No CurrentTeam Component Found on " + obj.gameObject.name);
        }
        if (_playerInputManager.playerCount % 2 == 0)
        {
            // Offense Spawn Logic
        }
        else
        {
            // Defense Spawn Logic
        }
        // Assumes that the player is spawned and play should start immediately
        obj.SwitchCurrentActionMap("Play");
    }
}
