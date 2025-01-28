using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletBehavior : BulletBehavior
{
    [SerializeField] float movementSpeedBoost = 2f;
    [SerializeField] float boostDurationS = 1f;

    private DefenseMovement defenseMovement;

    new protected void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < TagsOfBulletReseters.Length; i++)
        {
            if (other.CompareTag(TagsOfBulletReseters[i]))
            {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if (
                    hasTeam != null
                    && hasTeam.Team == this.gameObject.GetComponent<CurrentTeam>().Team
                )
                {
                    //if the bullet hits something on same team
                    if (other.CompareTag("BoostWall"))
                    {
                        speed *= 4;
                        damage *= 2;
                    }else if (other.CompareTag("Defense")) {
                        defenseMovement = other.gameObject.GetComponent<DefenseMovement>();
                        if (!defenseMovement.isBoosted) {
                            defenseMovement.isBoosted = true;
                            defenseMovement.speed *= movementSpeedBoost;
                            Invoke("RemoveBoost", boostDurationS);
                        }
                    }
                    break;
                }
                Health enemyHealth = other.gameObject.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.takeDamage(damage);
                }
                resetBullet();
                break;
            }
        }

    }
    void RemoveBoost() { 
        defenseMovement.isBoosted = false;
        defenseMovement.speed /= movementSpeedBoost;
    }
}
