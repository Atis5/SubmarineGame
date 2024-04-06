using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollecting : MonoBehaviour
{
    public GameObject TrashCollector;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TrashCollector)
        {
            PickUpItems.PickUpScript.SecureTrash();
        }
    }
}
