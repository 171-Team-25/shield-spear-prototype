using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField]
    public float speed = 10f;
    public float lifeTime = 2f;

    private float _lifeTimer;

    private BulletPool bulletPool;

    private string[] TagsOfBulletReseters = {"Offense", "Defense", "Enemy", "Shield"};
    // Start is called before the first frame update
    void Start()
    {
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
                    //if the bullet hits something on another team
                    break;
                }
                resetBullet();
                if(other.CompareTag("Enemy") || other.CompareTag("Shield")) {
                    Health enemyHealth = other.gameObject.GetComponent<Health>();
                    if (enemyHealth != null) {
                        enemyHealth.currentHealth -= 50;
                    }
                }
                break;
            }
        }
    }

    private void resetBullet() {
        bulletPool.ReturnBullet(gameObject);
        _lifeTimer = lifeTime;
    }

    public void SetPool(BulletPool pool) {
        bulletPool = pool;
    }

}
