using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VacuumWaste : MonoBehaviour
{
    [SerializeField] private Slider ProgressHandle;
    [SerializeField] private GameObject ToxicArea;
    void Start()
    {
        
    }

    void Update()
    {
        AreaInteraction();
        VacuumProgression();
    }

    //does not work atm
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            SubControl.SubControlScript.IsInToxicArea = true;
            Debug.Log("INSIDE TOXIC STUFF");
        }

    }

    //Should allow player to interact when inside of the toxic area
    void AreaInteraction()
    {
        if ((SubControl.SubControlScript.IsInToxicArea == true) && (Input.GetKey(Controls.ControlsScript.Vacuum)))
        {
            ProgressHandle.value++;
            //Debug.Log("Vacuum");
        }
        if ((SubControl.SubControlScript.IsInToxicArea == false) && (Input.GetKeyDown(Controls.ControlsScript.Vacuum)))
        {
            //Debug.Log("No Vacuuming allowed");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            SubControl.SubControlScript.IsInToxicArea = false;
        }
    }

    //retains progress of the vacuuming
    private void VacuumProgression()
    {
        if (ProgressHandle.value == 1)
        {
            GameObject.Destroy(ToxicArea);
        }
    }
    
 // private void ()
 // {
 //
 // }

}
