using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubControl : MonoBehaviour
{


    [Header("Properties")]
    [SerializeField] private float Acceleration; // How much will be added to the submarine speed every frame.
    [SerializeField] private float MaxForwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MaxBackwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MinSpeed; // If the SubmarineSpeed is below MinSpeed, it will instantly stop instead of going very slowly.
    [SerializeField] private float TurnSpeed; // How fast the submarine goes left and right.
    [SerializeField] private float RiseSpeed; // How fast the submarine goes up and down.
    [SerializeField] private float StabilizationSmoothing; // How fast the submarine will stabilize after being rotated.
    [SerializeField] private float BumpForce; // How far the submarine will be pushed away after hitting something.

    [Header("Necessary Variables")]
    public Collider SubmarineCollision;
    public static SubControl SubControlScript;
    private float SubmarineSpeed;
    private float CurrentBumpForce; 
    private Rigidbody rb;



    void Start()
    {
        SubControlScript = this; // Allows us to reference this script in other scripts.
        rb = GetComponent<Rigidbody>(); // Necessary reference to the RigidBody.
    }

    private void Update()
    {
        //Debug.Log(SubmarineSpeed);
    }

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
        if (Input.GetKey(Controls.ControlsScript.Forward))
        {
            SubmarineSpeed += Acceleration;
        }
        else if (Input.GetKey(Controls.ControlsScript.Backward))
        {
            SubmarineSpeed -= Acceleration;
        }
        
        else if (Mathf.Abs(SubmarineSpeed) <= MinSpeed)
        {
            SubmarineSpeed = 0;
        }
        
        SubmarineSpeed = Mathf.Clamp(SubmarineSpeed, -MaxBackwardSpeed, MaxForwardSpeed);
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
