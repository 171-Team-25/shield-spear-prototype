using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField]
    public float speed = 10f;
    private float baseSpeed;
    public float baselifeTime = 2f;

    public float lifeTime = 2f;

    private float _lifeTimer;

    private BulletPool bulletPool;
    public int damage = 50;
    private int baseDamage;

    public PlayerStats playerStats;

    protected string[] TagsOfBulletReseters =
    {
        "Offense",
        "Defense",
        "Enemy",
        "Shield",
        "BoostWall",
        "TerrainWall"
    };

    // Start is called before the first frame update
    protected void Start()
    {
        baseDamage = damage;
        baseSpeed = speed;
        OnStatChanges();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            resetBullet();
        }
    }

    protected void OnTriggerEnter(Collider other)
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

    protected void resetBullet()
    {
        bulletPool.ReturnBullet(gameObject);
        lifeTime = baselifeTime * playerStats.Distance;
        _lifeTimer = lifeTime;
        speed = baseSpeed;
        damage = (int)(baseDamage * playerStats.Damage);
    }

    public void SetPool(BulletPool pool)
    {
        bulletPool = pool;
    }

    public void OnStatChanges() {
        lifeTime = baselifeTime * playerStats.Distance;
        _lifeTimer = lifeTime;
        damage = (int)(baseDamage * playerStats.Damage);
    }
}
