using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VacuumWaste : MonoBehaviour
{
    [SerializeField] private float VacuumSpeed;
    [SerializeField] private Slider ProgressHandle;
    [SerializeField] private GameObject ToxicArea;
    [SerializeField] private GameObject Progress;
    void Start()
    {
        
    }

    void Update()
    {
        AreaInteraction();
        VacuumProgression();

  
    }


    // UNNECESSARY \/ \/ \/ 
    //does not work atm
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            SubControl.SubControlScript.IsInToxicArea = true;
            //Debug.Log("INSIDE TOXIC STUFF");
        }

    }*/

    //Should allow player to interact when inside of the toxic area
    void AreaInteraction()
    {
        if ((SubControl.SubControlScript.IsInToxicArea == true) && (Input.GetKey(Controls.ControlsScript.Vacuum)))
        {
            Progress.SetActive(true);
            ProgressHandle.value += VacuumSpeed * Time.deltaTime;
            //Debug.Log("Vacuum");
        }
        if ((SubControl.SubControlScript.IsInToxicArea == false) && (Input.GetKeyDown(Controls.ControlsScript.Vacuum)))
        {
            //Debug.Log("No Vacuuming allowed");
        }
    }


    // UNNECESSARY \/ \/ \/
    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            Progress.SetActive(false);
            SubControl.SubControlScript.IsInToxicArea = false;
        }
    }*/

 

    //retains progress of the vacuuming
    private void VacuumProgression()
    {
        if (ProgressHandle.value == 100)
        {
            //Progress.SetActive(false); <-- UNNECESSARY
            GameObject.Destroy(ToxicArea);
            SubControl.SubControlScript.IsInToxicArea = false;
        }

        if (SubControl.SubControlScript.IsInToxicArea == false)
        {
            Progress.SetActive(false);
        }
    }
    
 // private void ()
 // {
 //
 // }

}
