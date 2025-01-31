using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShotgunBlast : MonoBehaviour
{
    public BulletPool bulletPool;
    public Transform firePoint;
    public float fireRate = 10f;
    private float _fireCooldown;
    private PlayerInput _playerInput;
    [SerializeField] float blastAngleDegrees = 90f;
    [SerializeField] float numOfBullets = 5f;
    [SerializeField] Text abilityDisplay;

    void Start()
    {
        _fireCooldown = 0;
        GameObject bulletManager = transform.Find("ShotgunBulletPool").gameObject;
        if (bulletManager != null)
        {
            bulletPool = bulletManager.GetComponent<BulletPool>();
        }
        _playerInput = GetComponent<PlayerInput>();

    }

    void Update()
    {
        _fireCooldown -= Time.deltaTime;
        if (_fireCooldown <= 0 && _playerInput.actions["Ability2"].ReadValue<float>() > 0)
        {
            if (abilityDisplay.enabled) {
                abilityDisplay.enabled = false;
            }
            Shoot();
        }
        if (_fireCooldown <= 0 && !abilityDisplay.enabled) {
            abilityDisplay.enabled = true;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < numOfBullets; i++) {
            GameObject bullet = bulletPool.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.transform.Rotate(0,(-blastAngleDegrees/2) + (blastAngleDegrees/(numOfBullets - 1) * i),0);
            }
        }
        _fireCooldown = fireRate;
    }

}
