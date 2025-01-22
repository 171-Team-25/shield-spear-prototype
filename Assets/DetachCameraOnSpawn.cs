using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachCameraOnSpawn : MonoBehaviour
{
    public GameObject targetCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("No camera assigned to DetachCameraOnSpawn");
            return;
        }
        targetCamera.transform.SetParent(null, true);
    }
}
