using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayerCamera : MonoBehaviour
{
    [NonSerialized] private Camera _playerCamera;

    public GameObject playerCameraPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var playerCamera = Instantiate(playerCameraPrefab);
        var followTarget = playerCamera.GetComponent<FollowTarget>();
        followTarget.target = transform;
        _playerCamera = playerCamera.GetComponent<Camera>();
        GetComponent<PlayerInput>().camera = _playerCamera;
        Movement movement;
        if (TryGetComponent(out movement))
        {
            movement.offenseCamera = _playerCamera;
        }
    }
}
