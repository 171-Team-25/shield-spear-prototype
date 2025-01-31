using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected int baseHealth = 100;
    public int currentHealth;

    public bool IsWeakened = false;

    private Healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.Find("HealthbarCanvas") != null) {
            healthbar = transform.Find("HealthbarCanvas").gameObject.GetComponent<Healthbar>();
        }
        currentHealth = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        if (IsWeakened)
        {
            damage = (int)(damage * 1.5f);
        }
        Debug.Log(this + "takes " + damage + " damage");
        currentHealth -= damage;
        if (healthbar != null) {
            healthbar.UpdateHealthBar(baseHealth, currentHealth);
        }
    }
}
