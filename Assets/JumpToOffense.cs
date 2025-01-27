using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class JumpToOffense : MonoBehaviour
{
    private GameObject teammateOffense;
    private CurrentTeam defenseTeam;

    private PlayerInput _playerInput;

    private float jumpCooldown;
    [SerializeField] float jumpRate = 3f;
    private AbilityEffect disableMovement;
    private Rigidbody _rigidbody;
    private AbilitySystem abilitySystem;
    public float jumpDurationS;
    [SerializeField] float minimumDistanceToOffense = 3f;

    // Start is called before the first frame update
    void Start()
    {
        disableMovement = new AbilityEffect {
            EffectType = EffectType.DisableMovement
        };
        abilitySystem = GetComponent<AbilitySystem>();
        //abilitySystem.AddEffect(disableMovement);
        _rigidbody = GetComponent<Rigidbody>();
        defenseTeam = GetComponent<CurrentTeam>();
        findTeammate();
        _playerInput = GetComponent<PlayerInput>();
        jumpCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCooldown <= 0 && _playerInput.actions["Ability5"].ReadValue<float>() > 0) {
            if (Mathf.Max(0, Vector3.Distance(teammateOffense.transform.position, transform.position) - minimumDistanceToOffense) > 0) {
                Debug.Log("used ability 5");
                jumpCooldown = jumpRate;
                StartCoroutine(JumpToTeammate());
            }
        }
        jumpCooldown -= Time.deltaTime;

    }

    void findTeammate() {
        CurrentTeam[] players = FindObjectsOfType<CurrentTeam>();
        if (defenseTeam != null) {
            foreach(CurrentTeam player in players) {
                if(player.gameObject.tag == "Offense" && player.Team == defenseTeam.Team){
                    teammateOffense = player.gameObject;
                }
            }
        } 
    }

    private IEnumerator JumpToTeammate() {
        Collider defenseCollider = GetComponent<Collider>();
        if (defenseCollider != null) {
            defenseCollider.enabled = false;
        }
        var jumpTimer = jumpDurationS / Time.fixedDeltaTime;
        var jumpDistance = Mathf.Max(0, Vector3.Distance(teammateOffense.transform.position, transform.position) - minimumDistanceToOffense);
        var jumpSpeed =  jumpDistance / jumpTimer / Time.fixedDeltaTime;
        var jumpDirection = teammateOffense.transform.position - transform.position;
        var jumpDestination = teammateOffense.transform.position;
        jumpDirection.Normalize();
        abilitySystem.AddEffect(disableMovement);
        disableMovement.StartEffect();
        while (Mathf.Max(0, Vector3.Distance(jumpDestination, transform.position) - minimumDistanceToOffense) > 0) {
            jumpTimer--;
            _rigidbody.velocity = jumpDirection * jumpSpeed;
            yield return new WaitForFixedUpdate();
        }
        if (defenseCollider != null) {
            defenseCollider.enabled = true;
            Collider[] overlappingColliders = Physics.OverlapBox(defenseCollider.bounds.center, defenseCollider.bounds.extents, transform.rotation, ~0, QueryTriggerInteraction.Ignore);
            foreach (var collider in overlappingColliders) {
                if (collider != defenseCollider && collider.gameObject.CompareTag("Untagged")) {
                    _rigidbody.position = jumpDestination;
                    break;
                }
            }
        }
        disableMovement.EndEffect();
    }
}
