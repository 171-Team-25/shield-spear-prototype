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
    private Renderer matRenderer;
    private Color baseColor;
    public bool isDead = false;
    [SerializeField] float timeDeadS = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.Find("HealthbarCanvas") != null) {
            healthbar = transform.Find("HealthbarCanvas").gameObject.GetComponent<Healthbar>();
        }
        currentHealth = baseHealth;
        if (gameObject.tag == "Defense") {
            matRenderer = transform.Find("BodySize").gameObject.GetComponent<Renderer>();
        } else {
        matRenderer = GetComponent<Renderer>();
        }
        if (matRenderer != null) {
            baseColor = matRenderer.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            if (isDead) {
                return;
            }
            if (gameObject.tag == "Offense" || gameObject.tag == "Defense") {
                Die();
            } else {
                Destroy(gameObject);
            }
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

    void Die() {
        isDead = true;
        if (matRenderer != null) {
            Color newColor = Color.red;
            newColor.a = 0.3f;
            matRenderer.material.color = newColor;
        }
        Invoke("Revive", timeDeadS);
        if (tag == "Offense") {
            GetComponent<Collider>().enabled = false;
            GetComponent<ProjectileLauncher>().enabled = false;
            GetComponent<DashAbility>().enabled = false;
            GetComponent<AbilitySystem>().enabled = false;
            GetComponent<TugDefense>().enabled = false;
            GetComponent<ShotgunBlast>().enabled = false;
            GetComponent<PiercingBolt>().enabled = false;
        } else if (tag == "Defense") {
            GetComponent<Collider>().enabled = false;
            transform.Find("MeleeHitbox").GetComponent<MeleeAttack>().enabled = false;
            GetComponent<JumpToOffense>().enabled = false;
            transform.Find("ShieldHitbox").GetComponent<Shield>().enabled = false;
            transform.Find("BoostNBlockHitbox").GetComponent<BlockNBoostWall>().enabled = false;
            transform.Find("WeaknessZoneHitbox").GetComponent<WeaknessZone>().enabled = false;
            transform.Find("KnockbackHitbox").GetComponent<KnockbackPunch>().enabled = false;
        }
    }

    void Revive() {
        isDead = false;
        matRenderer.material.color = baseColor;
        currentHealth = baseHealth;
        if (tag == "Offense") {
            GetComponent<Collider>().enabled = true;
            GetComponent<ProjectileLauncher>().enabled = true;
            GetComponent<DashAbility>().enabled = true;
            GetComponent<AbilitySystem>().enabled = true;
            GetComponent<TugDefense>().enabled = true;
            GetComponent<ShotgunBlast>().enabled = true;
            GetComponent<PiercingBolt>().enabled = true;
        } else if (tag == "Defense") {
            GetComponent<Collider>().enabled = true;
            transform.Find("MeleeHitbox").GetComponent<MeleeAttack>().enabled = true;
            GetComponent<JumpToOffense>().enabled = true;
            transform.Find("ShieldHitbox").GetComponent<Shield>().enabled = true;
            transform.Find("BoostNBlockHitbox").GetComponent<BlockNBoostWall>().enabled = true;
            transform.Find("WeaknessZoneHitbox").GetComponent<WeaknessZone>().enabled = true;
            transform.Find("KnockbackHitbox").GetComponent<KnockbackPunch>().enabled = true;
        }
    }
}
