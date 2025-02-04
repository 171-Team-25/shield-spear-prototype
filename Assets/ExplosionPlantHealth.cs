using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ExplosionPlantHealth : Health
{
    private Collider plantCollider;
    private bool isExploding = false;
    private List<GameObject> hitEntities = new List<GameObject>();

    public float arcHeight = 2f;
    public float duration = 1f;
    
    public float distance = 15f;
    protected string[] TagsOfExplosionHittables =
    {
        "Offense",
        "Defense"
    };
    void Start()
    {
        plantCollider = GetComponent<Collider>();
        currentHealth = maxHealth;
        if (gameObject.tag == "Defense") {
            matRenderer = transform.Find("BodySize").gameObject.GetComponent<Renderer>();
        } else {
            matRenderer = GetComponent<Renderer>();
        }
        if (matRenderer != null) {
            baseColor = matRenderer.material.color;
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            if (isDead) {
                return;
            }
            Die();
        }
    }

    new void Die() {
        isDead = true;
        Invoke("Disappear", 0.25f);
        Explode();
        Invoke("Revive", timeDeadS);
        Invoke("Shrink", timeDeadS);
    }

    void Explode() {
        transform.localScale = new UnityEngine.Vector3(10, 0.5f, 10);
        isExploding = true;
    }

    void Disappear() {
        if (matRenderer != null) {
            matRenderer.enabled = false;
        }
        isExploding = false;
        hitEntities.Clear();
    }

    void Shrink() {
        transform.localScale = new UnityEngine.Vector3(1,1,1);
    }

    void Revive() {
        isDead = false;
        matRenderer.enabled = true;
        currentHealth = baseHealth;
    }

    private void OnTriggerEnter(Collider other) {
        if (isExploding) {
            for (int i = 0; i < TagsOfExplosionHittables.Length; i++)
            {
                if (other.CompareTag(TagsOfExplosionHittables[i]))
                {
                    if (!hitEntities.Contains(other.gameObject)) {
                        hitEntities.Add(other.gameObject);
                        StartCoroutine(BlastAway(other.gameObject));
                    }
                }
            }
            
        }
    }

    private IEnumerator BlastAway(GameObject blasted) {
        if (blasted.TryGetComponent<Movement>(out var movement))
        {
            blasted.GetComponent<Rigidbody>().velocity = UnityEngine.Vector3.zero;
            movement.enabled = false;
        }
        else if (blasted.TryGetComponent<DefenseMovement>(out var defenseMovement))
        {
            blasted.GetComponent<Rigidbody>().velocity = UnityEngine.Vector3.zero;
            defenseMovement.enabled = false;
        }
        Collider blastedCollider = blasted.GetComponent<Collider>();
        blastedCollider.enabled = false;

        UnityEngine.Vector3 start = blasted.transform.position;
        UnityEngine.Vector3 end = start + ((start - transform.position).normalized * distance);
        //new UnityEngine.Vector3(0,1,0);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            UnityEngine.Vector3 position = UnityEngine.Vector3.Lerp(start, end, t);

            position.y += arcHeight * Mathf.Sin(t * Mathf.PI);

            blasted.transform.position = position;
            yield return null;
        }

        blasted.transform.position = end;

        blastedCollider.enabled = true;
        if (blasted.TryGetComponent<Movement>(out var movement2))
        {
            movement2.enabled = true;
        }
        else if (blasted.TryGetComponent<DefenseMovement>(out var defenseMovement2))
        {
            defenseMovement2.enabled = true;
        }
    }
}
