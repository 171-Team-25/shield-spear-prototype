using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class WeaknessZone : MonoBehaviour
{
    private MeshCollider zoneCollider;
    private GameObject zoneVisual;
    private PlayerInput _playerInput;
    private bool zoneReady = true;
    private GameObject zoneOwner;
    private List<GameObject> hitEntities = new List<GameObject>();
    private string[] TagsOfHittables = {"Offense", "Defense", "Enemy"};

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        zoneOwner = transform.parent.gameObject;
        zoneCollider = GetComponent<MeshCollider>();
        zoneVisual = transform.Find("WeaknessZoneVisual").gameObject;
        CurrentTeam zoneMakersTeam = transform.parent.gameObject.GetComponent<CurrentTeam>();
        CurrentTeam zonesTeam = GetComponent<CurrentTeam>();
        if (zoneMakersTeam != null && zonesTeam != null) {
            zonesTeam.Team = zoneMakersTeam.Team;
        }
        transform.Find("WeaknessZoneVisual").gameObject.SetActive(false);
        transform.localPosition = new Vector3(0, 300, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (zoneReady && _playerInput.actions["Ability3"].ReadValue<float>() > 0) {
            zoneReady = false;
            transform.Find("WeaknessZoneVisual").gameObject.SetActive(true);
            transform.localPosition = new Vector3(0, -1, 4);
            Invoke("ResetZone", 2);
            transform.SetParent(null);
        }
    }

    void ResetZone() {
        zoneReady = true;
        transform.Find("WeaknessZoneVisual").gameObject.SetActive(false);
        transform.SetParent(zoneOwner.transform);
        transform.localPosition = new Vector3(0, 300, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        foreach(GameObject entitiy in hitEntities) {
            Health enemyHealth = entitiy.GetComponent<Health>();
            if (enemyHealth != null) {
                enemyHealth.IsWeakened = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < TagsOfHittables.Length; i++) {
            if (other.CompareTag(TagsOfHittables[i])) {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if ((hasTeam != null && hasTeam.Team != this.gameObject.GetComponent<CurrentTeam>().Team) || other.CompareTag("Enemy")) {
                    Health enemyHealth = other.gameObject.GetComponent<Health>();
                    if (enemyHealth != null && !enemyHealth.IsWeakened) {
                        if(!hitEntities.Contains(other.gameObject)) {
                            hitEntities.Add(other.gameObject);
                        }
                        enemyHealth.IsWeakened = true;
                    }
                }
            }
        }  
    }

    private void OnTriggerExit(Collider other) {
        for (int i = 0; i < TagsOfHittables.Length; i++) {
            if (other.CompareTag(TagsOfHittables[i])) {
                CurrentTeam hasTeam = other.gameObject.GetComponent<CurrentTeam>();
                if ((hasTeam != null && hasTeam.Team != this.gameObject.GetComponent<CurrentTeam>().Team) || other.CompareTag("Enemy")) {
                    Health enemyHealth = other.gameObject.GetComponent<Health>();
                    if (enemyHealth != null && enemyHealth.IsWeakened) {
                        if(hitEntities.Contains(other.gameObject)) {
                            hitEntities.Remove(other.gameObject);
                        }
                        enemyHealth.IsWeakened = false;
                    }
                }
            }
        }  
    }
}
