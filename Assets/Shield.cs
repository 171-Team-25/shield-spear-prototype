using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    private BoxCollider boxCollider;

    [SerializeField]
    float baseColliderHeight = 4f;

    [SerializeField]
    float baseColliderWidth = 5f;
    public float colliderHeight = 1f;
    public float colliderWidth = 1f;
    private GameObject shieldVisual;

    public bool shieldUp = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(colliderWidth, colliderHeight, 1);
            boxCollider.center = new Vector3(0, colliderHeight / 2 - 1, 0);
            boxCollider.enabled = false;
        }
        shieldVisual = transform.Find("ShieldVisual").gameObject;
        if (shieldVisual != null)
        {
            shieldVisual.transform.localPosition = new Vector3(0, colliderHeight / 2 - 1, 0);
            shieldVisual.transform.localScale = new Vector3(colliderWidth, colliderHeight, 1);
        }
                Debug.Log("area size start");

        CurrentTeam shieldbearersTeam = transform.parent.parent.gameObject.GetComponent<CurrentTeam>();
        CurrentTeam shieldsTeam = this.GetComponent<CurrentTeam>();
        if (shieldbearersTeam != null && shieldsTeam != null)
        {
            shieldsTeam.Team = shieldbearersTeam.Team;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldUp)
        {
            transform.Find("ShieldVisual").gameObject.SetActive(true);
        }
        else if (!shieldUp)
        {
            transform.Find("ShieldVisual").gameObject.SetActive(false);
        }
    }

    public void AreaSizeUpdated(float AreaSize) {
        colliderHeight = baseColliderHeight * AreaSize;
        colliderWidth = baseColliderWidth * AreaSize;
        Debug.Log("area size " + AreaSize + " " + colliderHeight);
        boxCollider = this.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Debug.Log("area sized change hitbox");
            boxCollider.size = new Vector3(colliderWidth, colliderHeight, 1);
            boxCollider.center = new Vector3(0, colliderHeight / 2 - 1, 0);
        }
        shieldVisual = transform.Find("ShieldVisual").gameObject;
        if (shieldVisual != null)
        {
            shieldVisual.transform.localPosition = new Vector3(0, colliderHeight / 2 - 1, 0);
            shieldVisual.transform.localScale = new Vector3(colliderWidth, colliderHeight, 1);
        }
    }
}
