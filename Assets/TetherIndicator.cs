using System;
using UnityEngine;

public class TetherIndicator : MonoBehaviour
{
    [NonSerialized] public Transform Offense;
    [NonSerialized] public Transform Defense;
    public float tetherThickness = 1f;
    public float MaxTetherDistance { get; set; }
    private const float MinTetherDistance = 1f;
    private Renderer _tetherRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _tetherRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Offense || !Defense) 
            return;

        Positioning();
        Scaling();
        Rotating();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Offense.position, MaxTetherDistance);
        Gizmos.color = Color.yellow;
    }

    void Positioning() {
        Vector3 midpoint = (Offense.position + Defense.position) / 2f;
        transform.position = midpoint;
    }

    void Scaling() {
        float distance = Math.Max(Vector3.Distance(Offense.position, Defense.position), float.Epsilon);
        transform.localScale = new Vector3(
            distance,
            Math.Min(1, 1 / distance) * tetherThickness,
            Math.Min(1, 1 / distance) * tetherThickness
        );
        Coloring(distance);
    }

    void Rotating() {
        Vector3 direction = (Offense.position - Defense.position).normalized;
        float zAngle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        float yAngle = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, yAngle, zAngle);
    }

    void Coloring(float distance) {
        distance = (distance - MinTetherDistance) / (MaxTetherDistance - MinTetherDistance);
        _tetherRenderer.material.SetColor("_Color", Color.Lerp(Color.green, Color.red, distance));
    }
}
