using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TugDefense : MonoBehaviour
{
    private PlayerInput _playerInput;
    private AbilityEffect disableMovement;
    private GameObject teammateDefense;
    private CurrentTeam offenseTeam;
    private float tugCooldown;
    [SerializeField] float tugRate = 3f;
    private Rigidbody defenseRigidbody;
    public float tugDurationS;
    [SerializeField] float minimumDistanceToDefense = 3f;

    // Start is called before the first frame update
    void Start()
    {
        disableMovement = new AbilityEffect { EffectType = EffectType.DisableMovement };
        _playerInput = GetComponent<PlayerInput>();
        offenseTeam = GetComponent<CurrentTeam>();
        tugCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (tugCooldown <= 0 && _playerInput.actions["Ability4"].ReadValue<float>() > 0) {
            Debug.Log("offense activate ability4");
            StartCoroutine(TugTeammate());  
        }
        tugCooldown -= Time.deltaTime;
    }

    void findTeammate() {
        CurrentTeam[] players = FindObjectsOfType<CurrentTeam>();
        if (offenseTeam != null) {
            foreach(CurrentTeam player in players) {
                if(player.gameObject.tag == "Defense" && player.Team == offenseTeam.Team){
                    teammateDefense = player.gameObject;
                }
            }
        }
        if (teammateDefense != null) {
            defenseRigidbody = teammateDefense.gameObject.GetComponent<Rigidbody>();
        }
    }

    private IEnumerator TugTeammate() {
        if (teammateDefense == null) {
            findTeammate();
            if (teammateDefense == null) {
                Debug.Log("no acceptable teammate");
                yield break;
            }
        }
        if (Mathf.Max(0, Vector3.Distance(teammateDefense.transform.position, transform.position) - minimumDistanceToDefense) <= 0) { 
            Debug.Log("too close to tug");
            yield break;
        }
        tugCooldown = tugRate;
        Collider defenseCollider = teammateDefense.GetComponent<Collider>();
        if (defenseCollider != null) {
            defenseCollider.enabled = false;
        }
        var tugTimer = tugDurationS / Time.fixedDeltaTime;
        var tugDistance = Mathf.Max(0, Vector3.Distance(teammateDefense.transform.position, transform.position) - minimumDistanceToDefense);
        var tugSpeed =  tugDistance / tugTimer / Time.fixedDeltaTime;
        var tugDirection = transform.position - teammateDefense.transform.position;
        var tugDestination = transform.position;
        tugDirection.Normalize();
        AbilitySystem defenseAbilitySystem = teammateDefense.GetComponent<AbilitySystem>();
        if (defenseAbilitySystem != null) {
            defenseAbilitySystem.AddEffect(disableMovement);
            disableMovement.StartEffect();
        }
        while (Mathf.Max(0, Vector3.Distance(tugDestination, teammateDefense.transform.position) - minimumDistanceToDefense) > 0) {
            tugTimer--;
            defenseRigidbody.velocity = tugDirection * tugSpeed;
            yield return new WaitForFixedUpdate();
        }
        if (defenseCollider != null) {
            defenseCollider.enabled = true;
            Collider[] overlappingColliders = Physics.OverlapBox(defenseCollider.bounds.center, defenseCollider.bounds.extents, transform.rotation, ~0, QueryTriggerInteraction.Ignore);
            foreach (var collider in overlappingColliders) {
                if (collider != defenseCollider && collider.gameObject.CompareTag("Untagged")) {
                    defenseRigidbody.position = tugDestination;
                    break;
                }
            }
        }
        if (defenseAbilitySystem != null) {
            disableMovement.EndEffect();
        }
    }
}
