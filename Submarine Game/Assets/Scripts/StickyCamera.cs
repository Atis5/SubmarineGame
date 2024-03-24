using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCamera : MonoBehaviour
{
    public Transform CameraPosition;
    void Update()
    {
        transform.position = CameraPosition.position;
    }
}
