using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField] float colliderHeight = 4f;
    [SerializeField] float colliderWidth = 5f;
    private GameObject shieldVisual;
    private PlayerInput _playerInput;
    private bool shieldUp = false;
    private bool pastShieldUp = true;
    private Vector3 shieldUpPos;
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        shieldUpPos = transform.localPosition;
        boxCollider = this.GetComponent<BoxCollider>();
        if (boxCollider != null) {
            boxCollider.size = new Vector3(colliderWidth, colliderHeight, 1);
            boxCollider.center = new Vector3(0, colliderHeight/2 - 1, 0);
        }
        shieldVisual = transform.Find("ShieldVisual").gameObject;
        if (shieldVisual != null) {
            shieldVisual.transform.localPosition = new Vector3(0, colliderHeight/2 - 1, 0);
            shieldVisual.transform.localScale =  new Vector3(colliderWidth, colliderHeight, 1);
        }
        CurrentTeam shieldbearersTeam = transform.parent.gameObject.GetComponent<CurrentTeam>();
        CurrentTeam shieldsTeam = this.GetComponent<CurrentTeam>();
        if (shieldbearersTeam != null && shieldsTeam != null) {
            shieldsTeam.Team = shieldbearersTeam.Team;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["Ability1"].ReadValue<float>() <= 0) {
            shieldUp = false;
        } else {
            shieldUp = true;
        }
        if (shieldUp && !pastShieldUp) {
            transform.Find("ShieldVisual").gameObject.SetActive(true);
            transform.localPosition = shieldUpPos;
            pastShieldUp = true;
        } else if (!shieldUp && pastShieldUp) {
            transform.Find("ShieldVisual").gameObject.SetActive(false);
            transform.localPosition = new Vector3(0,100,0);
            pastShieldUp = false;
        }
    }
}
