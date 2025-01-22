using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] int speed = 1;
    [SerializeField] SphereCollider maxDistanceFromDefense;
    [SerializeField] public Camera offenseCamera;
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null) {
            Debug.Log("no player input");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentYVelocity = body.velocity.y;
        var inputDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        body.velocity = new Vector3(inputDirection.x * speed, currentYVelocity, inputDirection.y * speed);
        body.position = ClampToDefense(body.position);
    }
    private void FixedUpdate() {
        RaycastHit hit;
        if (_playerInput.currentControlScheme == "Keyboard")
        {
            if (Physics.Raycast(offenseCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) 
            {
                if (hit.collider.tag == "Ground") 
                {
                    transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(hit.point - transform.position).eulerAngles.y, 0);
                }
            }
        }
        else
        {
            var dir = _playerInput.actions["Aim"].ReadValue<Vector2>();
            transform.rotation = Quaternion.LookRotation(new(dir.x, 0, dir.y));
        }
    }

    Vector3 ClampToDefense(Vector3 newPosition) {
        Vector3 defenseCenter = maxDistanceFromDefense.transform.position;
        Vector3 fromDefenseCenter = newPosition - defenseCenter;
        if (fromDefenseCenter.magnitude > maxDistanceFromDefense.radius) {
            fromDefenseCenter.Normalize();
            newPosition = defenseCenter + fromDefenseCenter * maxDistanceFromDefense.radius;
        }
        return newPosition;
    }
}
