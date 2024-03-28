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
        // Always start in first person.
        FirstPersonCamera.gameObject.SetActive(true);
        ThirdPersonCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(SubControl.Main.ChangeCamera))
        {
            FirstPerson = !FirstPerson;
            ChangeCameraPerspective();
        }
    }

    private void ChangeCameraPerspective()
    {
        if (FirstPerson == true)
        {
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
