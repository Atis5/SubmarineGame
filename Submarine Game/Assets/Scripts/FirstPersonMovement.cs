using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float PlayerSpeed;
    public float GroundDrag;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask GroundObjects;
    private bool Grounded;

    public Transform Orientation;

    private float HorizontalInput;
    private float VerticalInput;

    Vector3 MoveDirection;

    Rigidbody RigidBody;

    private void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
        RigidBody.freezeRotation = true;
    }

    private void Update()
    {
        // Check if Player is on the ground
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, GroundObjects);

        MyInput();
        SpeedControl();

        // Apply ground drag
        if (Grounded)
        {
            RigidBody.drag = GroundDrag;
        }
        else
        {
            RigidBody.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        MoveDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;
        RigidBody.AddForce(MoveDirection.normalized * PlayerSpeed * 10, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 FlatVelocity = new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z);

        if (FlatVelocity.magnitude > PlayerSpeed)
        {
            Vector3 LimitedVelocity = FlatVelocity.normalized * PlayerSpeed;
            RigidBody.velocity = new Vector3(LimitedVelocity.x, RigidBody.velocity.y, LimitedVelocity.z);
        }

    }
}
