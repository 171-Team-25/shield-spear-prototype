using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachCameraOnSpawn : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            Debug.LogWarning("No camera assigned to DetachCameraOnSpawn");
            return;
        }
        camera.transform.SetParent(null, true);
    }
}
