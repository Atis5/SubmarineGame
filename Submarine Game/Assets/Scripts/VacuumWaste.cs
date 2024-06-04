using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumWaste : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        AreaInteraction();
    }

    //does not work atm
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "toxic")
        {
            SubControl.SubControlScript.IsInToxicArea = true;
            Debug.Log("INSIDE TOXIC STUFF");
        }
    }

    //Should allow player to interact when inside of the toxic area
    void AreaInteraction()
    {
        if ((SubControl.SubControlScript.IsInToxicArea == true) && (Input.GetKeyDown(Controls.ControlsScript.Vacuum)))
        {
            Debug.Log("Vacuum");
        }
        if ((SubControl.SubControlScript.IsInToxicArea == false) && (Input.GetKeyDown(Controls.ControlsScript.Vacuum)))
        {
            Debug.Log("No Vacuming allowed");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            SubControl.SubControlScript.IsInToxicArea = false;
        }
    }
}
