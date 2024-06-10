using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualActivation : MonoBehaviour
{
    [SerializeField] private GameObject ManualActive;
    [SerializeField] private GameObject HUD;
    public bool IsManualActive;


    void Start()
    {
        ManualActive.SetActive(true);
        IsManualActive = true;
    }
    void Update()
    {
        ManualCheck();
    }

    private void ManualCheck()
    {
        if (Input.GetKeyDown(Controls.ControlsScript.Manual))
        {
            Debug.Log("manual");
            ManualActive.SetActive(true);
            HUD.SetActive(false);
        }
        if (Input.GetKeyUp(Controls.ControlsScript.Manual))
        {
            Debug.Log("manual off");
            ManualActive.SetActive(false);
            HUD.SetActive(true);
        }
    }
}
