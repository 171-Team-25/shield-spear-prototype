using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class JoinGameSystem : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private PlayerInputMap _playerInputMap;
    public bool fillTeamsInOrder = true;

    // Player Prefabs
    public GameObject offensePrefab;
    public GameObject defensePrefab;
    public GameObject teamIndicatorPrefab;

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
        if (
            ctx.control.device.name == "Keyboard"
            && _playerInputManager.playerCount < _playerInputManager.maxPlayerCount
        )
            Instantiate(_playerInputManager.playerPrefab);
        else
            _playerInputManager.JoinPlayer(pairWithDevice: ctx.control.device);
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log(playerInput);
        if (playerInput.devices.Count == 0)
        {
            Debug.Log("No Device Bound. Binding to keyboard.");
            var device = Keyboard.current;
            InputUser.PerformPairingWithDevice(device, playerInput.user);
        }

        var playerCount = _playerInputManager.playerCount;
        var teamSize = _playerInputManager.maxPlayerCount / 2;
        var team = fillTeamsInOrder ? (playerCount > teamSize ? 2 : 1) : (playerCount + 1) % 2 + 1;
        Debug.Log("Player " + _playerInputManager.playerCount + " has joined on team " + team);

        var playersTeamIndicator = Instantiate(teamIndicatorPrefab, playerInput.gameObject.transform);
        var indicatorRenderer = playersTeamIndicator.GetComponent<Renderer>();
        if (indicatorRenderer != null)
        {
            switch (team)
            {
                case 1:
                    indicatorRenderer.material.SetColor("_Color", Color.blue);
                    playersTeamIndicator.transform.localPosition = new UnityEngine.Vector3(0, -1, 0);
                    break;
                case 2:
                    indicatorRenderer.material.SetColor("_Color", Color.red);
                    playersTeamIndicator.transform.localPosition = new UnityEngine.Vector3(0, -1.1f, 0);
                    break;
            }
        }

        if (playerInput.gameObject.TryGetComponent<CurrentTeam>(out var currentTeam))
        {
            currentTeam.Team = team;
        }
        else
        {
            Debug.LogError("No CurrentTeam Component Found on " + playerInput.gameObject.name);
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
        playerInput.SwitchCurrentActionMap("Play");
        Debug.Log(playerInput.currentActionMap.name);
    }
}
