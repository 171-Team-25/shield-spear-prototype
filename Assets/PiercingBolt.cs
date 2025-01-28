using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PiercingBolt : MonoBehaviour
{
    private MeshCollider boltCollider;
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

    // Start is called before the first frame update
    void Start()
    {
        boltCooldown = 0;
        _playerInput = GetComponent<PlayerInput>();
        GameObject warningHitbox = transform.Find("PiercingBolt/PiercingBoltWarningHitbox").gameObject;
        warningVisual = warningHitbox.transform.Find("PiercingBoltWarningVisual").gameObject;
        warningRenderer = warningVisual.GetComponent<Renderer>();
        warningHitbox.transform.localPosition = new Vector3(0, -0.8f, boltDistance/2);
        warningVisual.transform.localScale = new Vector3(0.25f, 1, boltDistance/10);
        warningHitbox.GetComponent<BoxCollider>().size = new Vector3(2.5f, 1, boltDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (boltCooldown <= 0 && !isReadying && _playerInput.actions["Ability3"].ReadValue<float>() > 0) {
            isReadying = true;
        }
        Color warnColor = warningRenderer.material.color;
        if (isReadying) {
            warningTimer += Time.deltaTime;
            warnColor.a = Mathf.Lerp(0f, 1f, warningTimer/durationTillShoot);
            warningRenderer.material.color = warnColor;
            if (warningTimer >= durationTillShoot) {
                ShootingBolt();
                isReadying = false;
                isShooting = true;
            }
        } else {
            boltCooldown -= Time.deltaTime;
        }
        if (isShooting) {

        }
    }

    void ShootingBolt() {
        ResetBolt();
    }

    void ResetBolt() {
        boltCooldown = boltRate;
        isReadying = false;
        Color warnColor = warningRenderer.material.color;
        warnColor.a = 0f;
        warningRenderer.material.color = warnColor;
        warningTimer = 0;
    }

    IEnumerator TurnColorOff(Color warnColor) {
        yield return 0;
    }
}
