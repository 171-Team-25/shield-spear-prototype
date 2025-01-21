using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private GameObject[] bulletPool;
    // Start is called before the first frame update
    void Start()
    {
        if (bulletPrefab == null) {
            return;
        }

        bulletPool = new GameObject[poolSize];
        FillBulletPool(bulletPool, poolSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FillBulletPool(GameObject[] bulletPool, int poolSize) {
        for (int i = 0; i < poolSize; i++) {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
            if (bulletBehavior != null) {
                bulletBehavior.SetPool(this);
            }
            bulletPool[i] = bullet;
        }
    }

    public GameObject GetBullet() {
        foreach(GameObject bullet in bulletPool) {
            if (!bullet.activeInHierarchy) {
                bullet.SetActive(true);
                return bullet;
            }
        }
        return null;
    }

    public void ReturnBullet(GameObject bullet) {
        bullet.SetActive(false);
    }
}
