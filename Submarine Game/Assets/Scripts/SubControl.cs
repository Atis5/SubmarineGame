using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SubControl : MonoBehaviour
{


    [Header("Properties")]
    [SerializeField] private float Acceleration; // How much will be added to the submarine speed every frame.
                     public  float SubmarineSpeed = 0; // Influenced by Acceleration.
    [SerializeField] private float MaxForwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MaxBackwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MinSpeed; // How slow can Submarine be before stopping.
    [SerializeField] private float TurnSpeed; // How fast the submarine goes left and right.
    [SerializeField] private float RiseSpeed;   // How fast the submarine goes up and down.
    [SerializeField] private float StabilizationSmoothing; // How fast the submarine will stabilize after being rotated.
    [SerializeField] private float BumpForce; // How far the submarine will be pushed away after hitting something.

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI SubmarineSpeedText;
    [SerializeField] private TextMeshProUGUI SubmarineDepthText;
    [SerializeField] private TextMeshProUGUI ToxicityNumberText;

    [Header("Necessary Variables")]
    public GameObject Player;
    public Collider SubmarineCollision; // Needed to make colliders work.
    public GameObject ToxicityWarning;
    private bool ToxicityWarningSwitch = true;
    private float SubmarineDepth;
    private bool IsInToxicArea = false;
    private float ToxicityNumber = 10;
    public static SubControl SubControlScript; // Variable where the script reference is stored.
    private float CurrentBumpForce; // Influenced by BumpForce
    private Rigidbody rb; // Reference to Rigidbody.



    void Start()
    {
        SubControlScript = this; // Allows to reference this script in other scripts.
        rb = GetComponent<Rigidbody>(); // References Rigidbody component.
        SubmarineDepth = Player.transform.position.y;
        SubmarineDepthText.text = SubmarineDepth.ToString() + "m";
        SubmarineSpeedText.text = SubmarineSpeed.ToString();
        ToxicityNumberText.text = ToxicityNumber.ToString() + " TU";
    }   



    /*private void Update()
    {
        Debug.Log(SubmarineSpeed);
    }*/



    void FixedUpdate()
    {

        // Movement
        ForwardAndBackwardMovement();
        Turning();
        RisingAndSinking();

        // Submarine stabilization
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(new Vector3(0, rb.rotation.eulerAngles.y, 0)), StabilizationSmoothing));

        //HUD
        ShowToxicity();
        ShowDepth();

    }



    private void ForwardAndBackwardMovement()
    {
        // Submarine speed slowly increases by the amount of acceleration.
        if (Input.GetKey(Controls.ControlsScript.Forward))
        {
            SubmarineSpeed += Acceleration;
        }
        else if (Input.GetKey(Controls.ControlsScript.Backward))
        {
            SubmarineSpeed -= Acceleration;
        }
        
        // Stops submarine if speed is less than minimum. This is to prevent Submarine from moving very slowly and annoying players.
        else if (Mathf.Abs(SubmarineSpeed) <= MinSpeed)
        {
            SubmarineSpeed = 0;
        }
        
        SubmarineSpeed = Mathf.Clamp(SubmarineSpeed, -MaxBackwardSpeed, MaxForwardSpeed);
        SubmarineSpeedText.text = Mathf.Round(SubmarineSpeed).ToString() + " km/h";
        rb.AddForce(transform.forward * SubmarineSpeed);
    }



    private void Turning()
    {
        if (Input.GetKey(Controls.ControlsScript.TurnRight))
        {
            rb.AddTorque(transform.up * TurnSpeed);
        }
        else if (Input.GetKey(Controls.ControlsScript.TurnLeft))
        {
            rb.AddTorque(transform.up * -TurnSpeed);
        }
    }



    private void RisingAndSinking()
    {
        if (Input.GetKey(Controls.ControlsScript.Rise))
        {
            rb.AddForce(transform.up * RiseSpeed);
        }
        else if (Input.GetKey(Controls.ControlsScript.Sink))
        {
            rb.AddForce(transform.up * -RiseSpeed);
        }
    }


    private void ShowToxicity()
    {
        if ((IsInToxicArea == true) && (ToxicityNumber < 100))
        {
            ToxicityNumber += 0.2f;
            // Show toxicity meter
            ToxicityNumberText.text = Mathf.Round(ToxicityNumber).ToString() + " TU";
        }
        else if ((IsInToxicArea == false) && (ToxicityNumber > 10))
        {
            ToxicityNumber -= 0.2f;
            ToxicityNumberText.text = Mathf.Round(ToxicityNumber).ToString() + " TU";
        }

        if (ToxicityNumber >= 100)
        {
            InvokeRepeating("ShowToxicityWarning", 2f, 2f);
        }
        
    }

    private void ShowToxicityWarning()
    {
        ToxicityWarning.SetActive(ToxicityWarningSwitch);
        ToxicityWarningSwitch = !ToxicityWarningSwitch;
    }

    private void ShowDepth()
    {
        // Show submarine depth
        SubmarineDepth = Player.transform.position.y;
        SubmarineDepthText.text = Mathf.Round(SubmarineDepth - 1000).ToString() + " m";
    }

    // Bumping and tilting when hitting obstacles.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Pickable")
        {
            CurrentBumpForce = BumpForce * -SubmarineSpeed;
            SubmarineSpeed = 0;
            rb.AddForce(transform.forward * CurrentBumpForce);
            rb.AddTorque(transform.forward * -CurrentBumpForce * 2);
            rb.AddTorque(transform.up * -CurrentBumpForce);
            CurrentBumpForce = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            IsInToxicArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            IsInToxicArea = false;
        }
    }

}
