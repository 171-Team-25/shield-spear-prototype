using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefenseMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody body;

    [SerializeField]
    int speed = 1;
    public Camera defenseCamera;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentYVelocity = body.velocity.y;
        var inputDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        body.velocity = new Vector3(inputDirection.x * speed, 0, inputDirection.y * speed);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (_playerInput.currentControlScheme == "Keyboard")
        {
            if (
                Physics.Raycast(
                    defenseCamera.ScreenPointToRay(Input.mousePosition),
                    out hit,
                    Mathf.Infinity
                )
            )
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    transform.rotation = Quaternion.Euler(
                        0,
                        Quaternion.LookRotation(hit.point - transform.position).eulerAngles.y,
                        0
                    );
                }
            }
        }
        else
        {
            var dir = _playerInput.actions["Aim"].ReadValue<Vector2>();
            if (Math.Abs(dir.x) >= 0.3 || Math.Abs(dir.y) >= 0.3)
            {
                transform.rotation = Quaternion.LookRotation(new(dir.x, 0, dir.y));
            }
        }
    }
}
