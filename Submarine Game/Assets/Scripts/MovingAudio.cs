using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAudio : MonoBehaviour
{
    public AudioSource MovingSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Controls.ControlsScript.Forward))
        {
            MovingSound.enabled = true;
        }
        else
        {
            MovingSound.enabled = false;
        }
    }
}
