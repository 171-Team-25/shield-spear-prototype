using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public BulletPool bulletPool;
    public Transform firePoint;
    public float fireRate = 10f;
    private float _fireCooldown;
    // Start is called before the first frame update
    void Start()
    {
        _fireCooldown = 0;
        GameObject bulletManager = GameObject.Find("OffenseBulletPool");
        if (bulletManager != null) {
            bulletPool = bulletManager.GetComponent<BulletPool>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _fireCooldown -= Time.deltaTime;

        if(_fireCooldown <= 0 && Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        GameObject bullet = bulletPool.GetBullet();
        if (bullet != null) {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
        }
        _fireCooldown = fireRate;
    }
}
