using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBoltDamage : MonoBehaviour
{
    public List<GameObject> hitEntities = new List<GameObject>();
    [SerializeField] int damage = 150;
    private string[] TagsOfHittables = { "Offense", "Defense", "Enemy", "Shield" };
    private GameObject boltOwner;

    void Start() {
        boltOwner = transform.parent.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < TagsOfHittables.Length; i++)
        {
            if (other.CompareTag(TagsOfHittables[i]))
            {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if (
                    other.CompareTag("Enemy")
                    || (
                        hasTeam != null
                        && hasTeam.Team
                            != boltOwner.GetComponent<CurrentTeam>().Team
                    )
                )
                {
                    if (!hitEntities.Contains(other.gameObject))
                    {
                        Health enemyHealth = other.gameObject.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.takeDamage(damage);
                        }
                        hitEntities.Add(other.gameObject);
                    }
                }
            }
        }
    }
}
