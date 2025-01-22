using UnityEngine;
using UnityEngine.InputSystem;

public class JoinGameSystem : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private PlayerInputMap _playerInputMap;
    // Player Prefabs
    public GameObject offensePrefab;
    public GameObject defensePrefab;

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
        var team = (_playerInputManager.playerCount + 1) % 2;
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
        Debug.Log("Player " + _playerInputManager.playerCount + " has joined on team " + team);
    }
    
    private void OnPlayerJoined(PlayerInput obj)
    {
        var team = (_playerInputManager.playerCount + 1) % 2;
        if (_playerInputManager.playerCount % 2 == 0)
        {
            // Offense Spawn Logic
        }
        else
        {
            // Defense Spawn Logic
        }

    }
}
