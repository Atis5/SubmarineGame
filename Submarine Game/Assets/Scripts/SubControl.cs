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
    [SerializeField] private TextMeshProUGUI CurrentSpeedText;

    [Header("Necessary Variables")]
    public Collider SubmarineCollision; // Needed to make colliders work.
    public static SubControl SubControlScript; // Variable where the script reference is stored.
    private float CurrentBumpForce; // Influenced by BumpForce
    private Rigidbody rb; // Reference to Rigidbody.



    void Start()
    {
        SubControlScript = this; // Allows to reference this script in other scripts.
        rb = GetComponent<Rigidbody>(); // References Rigidbody component.
        CurrentSpeedText.text = SubmarineSpeed.ToString();
    }



    /*private void Update()
    {
        Debug.Log(SubmarineSpeed);
    }*/



    void FixedUpdate()
    {
        ForwardAndBackwardMovement();
        Turning();
        RisingAndSinking();

        // Submarine stabilization
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(new Vector3(0, rb.rotation.eulerAngles.y, 0)), StabilizationSmoothing));
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
        CurrentSpeedText.text = Mathf.Round(SubmarineSpeed / 10).ToString();
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


    // Bumping and tilting when hitting obstacles.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Pickable")
        {
            CurrentBumpForce = BumpForce * -SubmarineSpeed;
            SubmarineSpeed = 0;
            rb.AddForce(transform.forward * CurrentBumpForce);
            rb.AddTorque(transform.forward * -CurrentBumpForce);
            CurrentBumpForce = 0;
        }
    }

}
