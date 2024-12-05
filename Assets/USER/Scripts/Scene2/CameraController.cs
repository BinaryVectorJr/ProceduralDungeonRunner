using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetToFollow;
    public float xOffset, yOffset, zOffset;

    private void Update()
    {
        transform.position = targetToFollow.transform.position + new Vector3 (xOffset, yOffset, zOffset);
        transform.LookAt(targetToFollow.transform.position);
    }
}
