using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] CapsuleCollider meleeCollider;
    public float newHeight = 0f;
    [SerializeField] float maxHeight = 3f;
    [SerializeField] float meleeRate = 1f;
    private float meleeCooldown;
    private float meleeTimer = 0f;
    private bool isMeleeing = false;
    [SerializeField] float durationTillFull = 3f;
    [SerializeField] float durationTillEnd = 4f;
    private Stack<GameObject> hitEntities = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        meleeCooldown = 0;
        meleeCollider = this.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (meleeCooldown <= 0 && Input.GetMouseButtonDown(1)) {
            isMeleeing = true;
            meleeCooldown = meleeRate;
            meleeTimer = 0;
        }
        if (isMeleeing) {
            meleeTimer += Time.deltaTime;
            newHeight = Mathf.Lerp(0, maxHeight, Mathf.Clamp01(meleeTimer/durationTillFull));
            if (meleeTimer >= durationTillEnd) {
                isMeleeing = false;
                newHeight = 0;
                while (hitEntities.Count > 0) {
                    hitEntities.Pop();
                }
            }
        }
        meleeCooldown -= Time.deltaTime;
        meleeCollider.height = newHeight;
        transform.localPosition = new Vector3(0, 0, newHeight/2);
    }

    private void OnTriggerEnter(Collider other) {
        if(isMeleeing && other.CompareTag("Enemy")) {
            bool enemyNotYetHit = true;
            foreach (GameObject entity in hitEntities) {
                if (entity == other.gameObject) {
                    enemyNotYetHit = false;
                }
            }
            if (enemyNotYetHit) {
                Health enemyHealth = other.gameObject.GetComponent<Health>();
                if (enemyHealth != null) {
                    enemyHealth.health -= 50;
                }
                hitEntities.Push(other.gameObject);
            } 
        }
    }

}
