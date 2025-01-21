using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] int speed = 1;
    [SerializeField] SphereCollider maxDistanceFromDefense;
    [SerializeField] Camera offenseCamera;
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentYVelocity = body.velocity.y;
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        body.velocity = new Vector3(inputDirection.x * speed, currentYVelocity, inputDirection.z * speed);
        body.position = ClampToDefense(body.position);
    }
    private void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(offenseCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            if (hit.collider.tag == "Ground") {
                transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(hit.point - transform.position).eulerAngles.y, 0);
            }
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
