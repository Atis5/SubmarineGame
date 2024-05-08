using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCamera : MonoBehaviour
{


    [Header("Camera")]
    [SerializeField] private Camera FirstPersonCamera;
    [SerializeField] private Camera ThirdPersonCamera;
    private bool FirstPerson = true;

    void Start()
    {
        Cursor.visible = false;

        // Always start in first person.
        FirstPersonCamera.gameObject.SetActive(true);
        ThirdPersonCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(Controls.ControlsScript.ChangeCamera))
        {
            FirstPerson = false;
            ChangeCameraPerspective();
        }
        if (Input.GetKeyUp(Controls.ControlsScript.ChangeCamera))
        {
            FirstPerson = true;
            ChangeCameraPerspective();
        }
    }

    private void ChangeCameraPerspective()
    {
        if (FirstPerson == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            FirstPersonCamera.gameObject.SetActive(true);
            ThirdPersonCamera.gameObject.SetActive(false);
        }
        else
        {
            FirstPersonCamera.gameObject.SetActive(false);
            ThirdPersonCamera.gameObject.SetActive(true);
        }
    }

}
