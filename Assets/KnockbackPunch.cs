using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KnockbackPunch : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider knockbackCollider;
    public float newHeight = 0f;

    [SerializeField]
    float maxHeight = 3f;

    [SerializeField]
    float knockbackRate = 1f;
    private float knockbackCooldown;
    private float knockbackTimer = 0f;
    private bool isKnockbacking = false;

    [SerializeField]
    float durationTillFull = 3f;
    private List<GameObject> hitEntities = new List<GameObject>();
    private GameObject visualizer;
    private float visualizerLength;

    private PlayerInput _playerInput;
    private float charge = 0;
    private float chargeRate = 1.2f;
    private string[] TagsOfHittables = { "Offense", "Defense", "Enemy" };
    private AbilityEffect disableMovement;

    [SerializeField] Text abilityDisplay;


    // Start is called before the first frame update
    void Start()
    {
        disableMovement = new AbilityEffect { EffectType = EffectType.DisableMovement };
        _playerInput = transform.parent.gameObject.GetComponent<PlayerInput>();
        visualizer = transform.Find("KnockbackVisual").gameObject;
        knockbackCooldown = 0;
        knockbackCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackCooldown <= 0 && _playerInput.actions["Ability4"].ReadValue<float>() > 0)
        {
            charge = Mathf.Min(4, charge + (chargeRate * Time.deltaTime));
            visualizer.transform.localScale = new Vector3(1, (maxHeight + charge) / 2, 1);
            
            abilityDisplay.enabled = false;
        }
        if (
            !isKnockbacking
            && _playerInput.actions["Ability4"].ReadValue<float>() <= 0
            && charge > 0
        )
        {
            isKnockbacking = true;
            knockbackCooldown = knockbackRate;
            knockbackTimer = 0;
        }
        if (isKnockbacking)
        {
            knockbackTimer += Time.deltaTime;
            newHeight = Mathf.Lerp(
                0,
                maxHeight + charge,
                Mathf.Clamp01(knockbackTimer / durationTillFull)
            );
            visualizerLength = Mathf.Lerp(
                0.5f,
                (maxHeight + charge) / 2,
                Mathf.Clamp01(knockbackTimer / durationTillFull)
            );
            if (knockbackTimer >= durationTillFull)
            {
                isKnockbacking = false;
                charge = 0;
                newHeight = 0;
                visualizerLength = 0.5f;
                if (hitEntities.Count > 0)
                {
                    hitEntities.Clear();
                }
            }
            knockbackCollider.height = newHeight;
            transform.localPosition = new Vector3(0, 0, newHeight);
            visualizer.transform.localScale = new Vector3(1, visualizerLength, 1);
        }
        if (!isKnockbacking && charge <= 0)
        {
            knockbackCooldown -= Time.deltaTime;
        }
        if (knockbackCooldown <= 0 && !abilityDisplay.enabled && charge <= 0) {
            abilityDisplay.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < TagsOfHittables.Length; i++)
        {
            if (isKnockbacking && other.CompareTag(TagsOfHittables[i]))
            {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if (
                    other.CompareTag("Enemy")
                    || (
                        hasTeam != null
                        && hasTeam.Team
                            != transform.parent.gameObject.GetComponent<CurrentTeam>().Team
                    )
                )
                {
                    if (!hitEntities.Contains(other.gameObject))
                    {
                        Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
                        if (enemyRB != null)
                        {
                            if (
                                other.gameObject.TryGetComponent<AbilitySystem>(
                                    out var abilitySystem
                                )
                            )
                            {
                                abilitySystem.AddEffect(disableMovement);
                                disableMovement.StartEffect();
                                enemyRB.velocity =
                                    transform.parent.transform.forward * (charge * 5);
                                StartCoroutine(EndEffectAfterTime(disableMovement));
                            }
                            ;
                        }
                        hitEntities.Add(other.gameObject);
                    }
                }
            }
        }
    }

    IEnumerator EndEffectAfterTime(AbilityEffect effect)
    {
        yield return new WaitForSeconds(2);
        effect.EndEffect();
    }
}
