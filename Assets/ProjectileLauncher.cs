using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileLauncher : MonoBehaviour
{
    public BulletPool bulletPool;
    public Transform firePoint;
    public float fireRate = 10f;
    private float _fireCooldown;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _fireCooldown = 0;
        GameObject bulletManager = transform.Find("OffenseBulletPool").gameObject;
        if (bulletManager != null)
        {
            bulletPool = bulletManager.GetComponent<BulletPool>();
        }
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0 && _playerInput.actions["Attack"].ReadValue<float>() > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = bulletPool.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
        }
        _fireCooldown = fireRate;
    }
}
