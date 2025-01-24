using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class KnockbackPunch : MonoBehaviour
{
    [SerializeField] CapsuleCollider knockbackCollider;
    public float newHeight = 0f;
    [SerializeField] float maxHeight = 3f;
    [SerializeField] float knockbackRate = 1f;
    private float knockbackCooldown;
    private float knockbackTimer = 0f;
    private bool isKnockbacking = false;

    [SerializeField] float durationTillFull = 3f;
    private List<GameObject> hitEntities = new List<GameObject>();
    private GameObject visualizer;
    private float visualizerLength;

    private PlayerInput _playerInput;
    private float charge = 0;
    private float chargeRate = 1f;
    private string[] TagsOfHittables = {"Offense", "Defense", "Enemy"};
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.gameObject.GetComponent<PlayerInput>();
        visualizer = transform.Find("KnockbackVisual").gameObject;
        knockbackCooldown = 0;
        knockbackCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackCooldown <= 0 && _playerInput.actions["Ability4"].ReadValue<float>() > 0) {
            charge += chargeRate * Time.deltaTime;
            Debug.Log("charge" + charge);
        }
        if (_playerInput.actions["Ability4"].ReadValue<float>() <= 0 && charge > 0) {
           isKnockbacking = true;
           knockbackCooldown = knockbackRate;
           knockbackTimer = 0;
        }
        if (isKnockbacking) {
            knockbackTimer += Time.deltaTime;
            newHeight = Mathf.Lerp(0, maxHeight, Mathf.Clamp01(knockbackTimer/durationTillFull));
            visualizerLength = Mathf.Lerp(0.5f, maxHeight/2, Mathf.Clamp01(knockbackTimer/durationTillFull));
            if (knockbackTimer >= durationTillFull) {
                isKnockbacking = false;
                charge = 0;
                newHeight = 0;
                visualizerLength = 0.5f;
                if (hitEntities.Count > 0) {
                    hitEntities.Clear();
                }
            }
        }
        if (!isKnockbacking && charge <= 0) {
            knockbackCooldown -= Time.deltaTime;
        }
        knockbackCollider.height = newHeight;
        transform.localPosition = new Vector3(0, 0, newHeight);
        visualizer.transform.localScale = new Vector3(1, visualizerLength, 1);
    }
}
