using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherIndicator : MonoBehaviour
{
    [SerializeField] Transform offense;
    [SerializeField] Transform defense;
    [SerializeField] Renderer tetherRenderer;
    [SerializeField] SphereCollider tetherRangeCollider;
    [SerializeField] float maxTetherDistance;
    [SerializeField] float minTetherDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        tetherRenderer = this.GetComponent<Renderer>();
        if (tetherRangeCollider != null) {
            maxTetherDistance = tetherRangeCollider.radius;
        } else {
            maxTetherDistance = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (offense == null || defense == null) return;

        Positioning();
        Scaling();
        Rotating();
    }

    void Positioning() {
        Vector3 midpoint = (offense.position + defense.position) / 2f;
        transform.position = midpoint;
    }

    void Scaling() {
        float distance = Vector3.Distance(offense.position, defense.position);
        transform.localScale = new Vector3(distance, Math.Min(1, 1/distance), Math.Min(1, 1/distance));
        Coloring(distance);
    }

    void Rotating() {
        Vector3 direction = (offense.position - defense.position).normalized;
        float zAngle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        float yAngle = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, yAngle, zAngle);
    }

    void Coloring(float distance) {
        distance = (distance - minTetherDistance) / (maxTetherDistance - minTetherDistance);
        tetherRenderer.material.SetColor("_Color", Color.Lerp(Color.green, Color.red, distance));
    }
}
