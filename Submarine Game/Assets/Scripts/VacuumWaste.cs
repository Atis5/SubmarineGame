using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumWaste : MonoBehaviour
{
    private bool IsInToxicArea = false;
    void Start()
    {
        
    }

    void Update()
    {
        AreaInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "toxic")
        {
            IsInToxicArea = true;
        }
    }

    //Should allow player to interact when inside of the toxic area
    void AreaInteraction()
    {
        if ((IsInToxicArea = true) && (Input.GetKeyDown(Controls.ControlsScript.Vacuum)))
        {
            
        }
    }
}
