using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField] float colliderHeight = 4f;
    [SerializeField] float colliderWidth = 5f;
    private GameObject shieldVisual;
    // Start is called before the first frame update
    void Start()
    {
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
            shieldsTeam.CurrentTeamOn = shieldbearersTeam.CurrentTeamOn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
