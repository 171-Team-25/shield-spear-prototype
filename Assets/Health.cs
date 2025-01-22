using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int baseHealth = 100;
    public int currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            Destroy(this.gameObject);
        }
    }
}
