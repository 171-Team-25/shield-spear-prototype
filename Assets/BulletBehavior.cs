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
    public float lifeTime = 2f;

    private float _lifeTimer;

    private BulletPool bulletPool;
    public int damage = 50;
    private int baseDamage;

    private string[] TagsOfBulletReseters = {"Offense", "Defense", "Enemy", "Shield", "BoostWall"};
    // Start is called before the first frame update
    void Start()
    {
        baseDamage = damage;
        baseSpeed = speed;
        _lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f) {
            resetBullet();
        }
    }
    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < TagsOfBulletReseters.Length; i++) {
            if (other.CompareTag(TagsOfBulletReseters[i])) {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if (hasTeam != null && hasTeam.Team == this.gameObject.GetComponent<CurrentTeam>().Team) {
                    //if the bullet hits something on same team
                    if (other.CompareTag("BoostWall")) {
                        speed *= 4;
                        damage *= 2;
                    }
                    break;
                }
                resetBullet();
                Health enemyHealth = other.gameObject.GetComponent<Health>();
                if (enemyHealth != null) {
                    enemyHealth.takeDamage(damage);
                }
                break;
            }
        }
    }

    private void resetBullet() {
        bulletPool.ReturnBullet(gameObject);
        _lifeTimer = lifeTime;
        speed = baseSpeed;
        damage = baseDamage;
    }

    public void SetPool(BulletPool pool) {
        bulletPool = pool;
    }

}
