using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class BlockNBoostWall : MonoBehaviour
{
    private BoxCollider boxCollider;

    [SerializeField]
    float colliderHeight = 6f;

    [SerializeField]
    float colliderWidth = 3f;
    private GameObject wallVisual;
    private PlayerInput _playerInput;
    private bool wallReady = true;
    private GameObject wallOwner;
    
    [SerializeField] Text abilityDisplay;


    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        wallOwner = transform.parent.gameObject;
        boxCollider = this.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(colliderWidth, colliderHeight, 1);
            boxCollider.center = new Vector3(0, colliderHeight / 2 - 1, 0);
        }
        wallVisual = transform.Find("BoostNBlockVisual").gameObject;
        if (wallVisual != null)
        {
            wallVisual.transform.localPosition = new Vector3(0, colliderHeight / 2 - 1, 0);
            wallVisual.transform.localScale = new Vector3(colliderWidth, colliderHeight, 1);
        }
        CurrentTeam wallMakersTeam = transform.parent.gameObject.GetComponent<CurrentTeam>();
        CurrentTeam wallsTeam = this.GetComponent<CurrentTeam>();
        if (wallMakersTeam != null && wallsTeam != null)
        {
            wallsTeam.Team = wallMakersTeam.Team;
        }
        transform.Find("BoostNBlockVisual").gameObject.SetActive(false);
        transform.localPosition = new Vector3(0, 200, 0);
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wallReady && _playerInput.actions["Ability2"].ReadValue<float>() > 0)
        {
            wallReady = false;
            transform.Find("BoostNBlockVisual").gameObject.SetActive(true);
            transform.localPosition = new Vector3(0, 0, 3);
            boxCollider.enabled = true;
            Invoke("ResetWall", 2);
            transform.SetParent(null);
            abilityDisplay.enabled = false;
        }
    }

    void ResetWall()
    {
        wallReady = true;
        transform.Find("BoostNBlockVisual").gameObject.SetActive(false);
        transform.SetParent(wallOwner.transform);
        boxCollider.enabled = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        abilityDisplay.enabled = true;
    }
}
