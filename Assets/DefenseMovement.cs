using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefenseMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody body;

    public float speed = 1;
    public Camera defenseCamera;
    private PlayerInput _playerInput;

    public bool isBoosted = false;
    [SerializeField] private float tetherDistance = 10f;
    [SerializeField] private float tetherPullForceFactor = 1f;
    [SerializeField] private float maxTetherPullForce = 50f;
    [SerializeField] private float tetherDistanceBuffer = 1f;
    public float TetherDistance { get => tetherDistance; set => tetherDistance = value; }
    public TetherIndicator Tether { get; set; }

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
        ApplyTether();
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
    private void ApplyTether() 
    {
         if (!Tether)
             return;
         else
         {
             Tether.MinTetherDistance = Tether.MaxTetherDistance - tetherDistanceBuffer;
         }
         var distance = Vector3.Distance(transform.position, Tether.Offense.position);
         var direction = (Tether.Offense.position - transform.position).normalized;
         if (distance + tetherDistanceBuffer < Tether.MaxTetherDistance)
             return;
 
         // Calculate Pulling Force
         // Function f(x) = -f * log((m - x) / m) - (m - b) / m
         // Break up equation
         var bufferFraction = (Tether.MaxTetherDistance - tetherDistanceBuffer) / Tether.MaxTetherDistance;
         var distanceFraction = (Tether.MaxTetherDistance - distance) / Tether.MaxTetherDistance;
         var force = -1 * Mathf.Log(Math.Max(Mathf.Epsilon, distanceFraction)) - bufferFraction;
         // Apply Force Scaler
         force *= tetherPullForceFactor;
         // Clamp Pull Force
         force = Mathf.Min(force, maxTetherPullForce);
         body.velocity += direction * force;
         Debug.Log(distance + " " + force);
     }
}
