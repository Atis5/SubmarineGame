using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI CurrentSpeedText;
    //[SerializeField] private TextMeshProUGUI CurrentDepthText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(SubControl.SubControlScript.SubmarineSpeed);
        CurrentSpeedText.text = SubControl.SubControlScript.SubmarineSpeed.ToString();
    }
}
