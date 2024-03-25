using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineControl : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float Acceleration;
    [SerializeField] private float MaxForwardSpeed;
    [SerializeField] private float MaxBackwardSpeed;
    [SerializeField] private float MinSpeed;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float RiseSpeed;
    [SerializeField] private float StabilizationSmoothing;

    [Header("Controls")]
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode Rise;
    public KeyCode Sink;

    [Header("Necessary Variables")]
    private float SubmarineSpeed;
    private Rigidbody rb;

    [Header("Camera")]
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;
    private bool firstPerson = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(SubmarineSpeed);
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            firstPerson = !firstPerson;
            ChangeCameraPerspective();
        }
    }

    private void ChangeCameraPerspective()
    {
        if (firstPerson == true)
        {
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
        } 
        else 
        {
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
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

}
