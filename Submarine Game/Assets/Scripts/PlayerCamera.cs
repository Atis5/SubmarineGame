using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float SensivityX;
    public float SensivityY;

    public Transform Orientation;

    private float RotationX;
    private float RotationY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensivityX;
        float MouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensivityY;
        RotationY += MouseX;
        RotationX -= MouseY;
        RotationX = Mathf.Clamp(RotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(RotationX, RotationY, 0);
        Orientation.rotation = Quaternion.Euler(0, RotationY, 0);
    }
}
