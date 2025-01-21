using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    [SerializeField] GameObject offense;
    [SerializeField] GameObject defense;
    // Start is called before the first frame update
    void Start()
    {
        Collider colliderA = offense.GetComponent<Collider>();
        Collider colliderB = defense.GetComponent<Collider>();

        Physics.IgnoreCollision(colliderA, colliderB);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
