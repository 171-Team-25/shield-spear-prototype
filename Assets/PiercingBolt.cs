using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PiercingBolt : MonoBehaviour
{
    private Renderer warningRenderer;
    private PlayerInput _playerInput;
    private float boltCooldown;
    private float warningTimer = 0f;
    private float boltTimer = 0f;
    [SerializeField] float boltRate = 2f;

    [SerializeField] float durationTillShoot = 3f;
    [SerializeField] float durationTillFull = 0.5f;
    private bool isReadying = false;
    private bool isShooting = false;

    [SerializeField] float boltDistance = 10f;

    private GameObject warningVisual;

    private GameObject boltVisual;
    private GameObject boltHitbox;
    private CapsuleCollider boltCollider;
    private GameObject piercingBolt;
    // Start is called before the first frame update
    void Start()
    {
        boltCooldown = 0;
        _playerInput = GetComponent<PlayerInput>();
        piercingBolt = transform.Find("PiercingBolt").gameObject;
        GameObject warningHitbox = piercingBolt.transform.Find("PiercingBoltWarningHitbox").gameObject;
        warningVisual = warningHitbox.transform.Find("PiercingBoltWarningVisual").gameObject;
        warningRenderer = warningVisual.GetComponent<Renderer>();
        warningHitbox.transform.localPosition = new Vector3(0, -0.8f, boltDistance/2);
        warningVisual.transform.localScale = new Vector3(0.25f, 1, boltDistance/10);
        warningHitbox.GetComponent<BoxCollider>().size = new Vector3(2.5f, 1, boltDistance);
        boltHitbox = piercingBolt.transform.Find("PiercingBoltHitbox").gameObject;
        boltVisual = boltHitbox.transform.Find("PiercingBoltVisual").gameObject;
        boltCollider = boltHitbox.GetComponent<CapsuleCollider>();
        boltCollider.enabled = false;
        ExtendBolt(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (boltCooldown <= 0 && !isReadying && _playerInput.actions["Ability3"].ReadValue<float>() > 0) {
            isReadying = true;
            piercingBolt.transform.SetParent(null);
        }
        Color warnColor = warningRenderer.material.color;
        if (isReadying) {
            warningTimer += Time.deltaTime;
            warnColor.a = Mathf.Lerp(0f, 1f, warningTimer/durationTillShoot);
            warningRenderer.material.color = warnColor;
            if (warningTimer >= durationTillShoot) {
                isReadying = false;
                isShooting = true;
                boltCollider.enabled = true;
            }
        } else {
            boltCooldown -= Time.deltaTime;
        }
        if (isShooting) {
            boltTimer += Time.deltaTime;
            float height = Mathf.Lerp(0, boltDistance, boltTimer/durationTillFull);
            ExtendBolt(height);
            if(boltTimer >= durationTillFull) {
                isShooting = false;
                boltCollider.enabled = false;
                ResetBolt();
            }
        }
    }

    void ResetBolt() {
        boltCooldown = boltRate;
        isReadying = false;
        Color warnColor = warningRenderer.material.color;
        warnColor.a = 0f;
        warningRenderer.material.color = warnColor;
        warningTimer = 0;
        boltTimer = 0;
        ExtendBolt(0f);
        piercingBolt.transform.SetParent(transform);
        piercingBolt.transform.localRotation = Quaternion.Euler(0, 0, 0);
        piercingBolt.transform.localPosition = new Vector3(0,0,0);
    }

    void ExtendBolt(float height) {
        boltCollider.height = height;
        boltHitbox.transform.localPosition = new Vector3(0,0,height/2);
        boltVisual.transform.localScale = new Vector3(1,height/2, 1);
    }
}
