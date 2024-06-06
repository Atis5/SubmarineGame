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

    //Should allow player to interact when inside of the toxic area
    void AreaInteraction()
    {
        if ((SubControl.SubControlScript.IsInToxicArea == true) && (Input.GetKey(Controls.ControlsScript.Vacuum)))
        {
            Progress.SetActive(true);
            ProgressHandle.value += VacuumSpeed * Time.deltaTime;
        }
    }

    //retains progress of the vacuuming
    private void VacuumProgression()
    {
        if (ProgressHandle.value == 100)
        {
            ToxicArea.SetActive(false);
            SubControl.SubControlScript.IsInToxicArea = false;
        }

        if (SubControl.SubControlScript.IsInToxicArea == false)
        {
            Progress.SetActive(false);
        }
    }
}
