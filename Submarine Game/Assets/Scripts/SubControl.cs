using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubControl : MonoBehaviour
{


    [Header("Properties")]
    [SerializeField] private float Acceleration;
    [SerializeField] private float MaxForwardSpeed;
    [SerializeField] private float MaxBackwardSpeed;
    [SerializeField] private float MinSpeed;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float RiseSpeed;
    [SerializeField] private float StabilizationSmoothing;
    [SerializeField] private float BumpForce;
    

    [Header("Controls")]
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode Rise;
    public KeyCode Sink;
    public KeyCode ChangeCamera;

    [Header("Necessary Variables")]
    public Collider SubmarineCollision;
    // "public static SubControl Main" allows us to reference this script in other scripts.
    public static SubControl Main;
    private float SubmarineSpeed;
    private float CurrentBumpForce;
    private Rigidbody rb;



    void Start()
    {
        Main = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Debug.Log(SubmarineSpeed);

        if (Input.GetKeyDown(ChangeCamera))
        { 
        }
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
        if (Input.GetKey(Forward))
        {
            SubmarineSpeed += Acceleration;
        }
        else if (Input.GetKey(Backward))
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
        if (Input.GetKey(TurnRight))
        {
            rb.AddTorque(transform.up * TurnSpeed);
        }
        else if (Input.GetKey(TurnLeft))
        {
            rb.AddTorque(transform.up * -TurnSpeed);
        }
    }

    private void RisingAndSinking()
    {
        if (Input.GetKey(Rise))
        {
            rb.AddForce(transform.up * RiseSpeed);
        }
        else if (Input.GetKey(Sink))
        {
            rb.AddForce(transform.up * -RiseSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CurrentBumpForce = BumpForce * -SubmarineSpeed;
        SubmarineSpeed = 0;
        rb.AddForce(transform.forward * CurrentBumpForce);
        CurrentBumpForce = 0;
    }

}
