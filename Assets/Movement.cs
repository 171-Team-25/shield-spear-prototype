using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] int speed = 1;
    [SerializeField] public Camera offenseCamera;
    private PlayerInput _playerInput;
    [SerializeField] private float tetherDistance = 100f;
    [SerializeField] private float tetherPullForceFactor = 1f;
    [SerializeField] private float tetherDistanceBuffer = 50f;
    public float TetherDistance { get => tetherDistance; set => tetherDistance = value; }
    public TetherIndicator Tether { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null)
        {
            Debug.Log("no player input");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var currentYVelocity = body.velocity.y;
        var inputDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        body.velocity = new Vector3(inputDirection.x * speed, 0, inputDirection.y * speed);
        ApplyTether();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (_playerInput.currentControlScheme == "Keyboard")
        {
            if (
                Physics.Raycast(
                    offenseCamera.ScreenPointToRay(Input.mousePosition),
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

    private void ApplyTether() {
        if (!Tether)
            return;
        var distance = Vector3.Distance(transform.position, Tether.Defense.position);
        if (distance - tetherDistanceBuffer <= 0)
            return;
        
        var force = 1 - Mathf.Pow(1 - distance / tetherDistanceBuffer, 3 );
        Debug.Log(force);
    }
}
