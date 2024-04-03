using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{

    [SerializeField] private Transform CameraTransform;
    [SerializeField] private LayerMask PickableObjects;


    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            float PickUpDistance = 2f;
            Physics.Raycast(CameraTransform.position, CameraTransform.forward, out RaycastHit raycastHit, PickUpDistance);
        }
    }
}
