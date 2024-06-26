using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{  
    public static Controls ControlsScript;

    [Header("Controls")]
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode TurnUp;
    public KeyCode TurnDown;
    public KeyCode Rise;
    public KeyCode Sink;
    public KeyCode ChangeCamera;
    public KeyCode PickUp;
    public KeyCode Defense;
    public KeyCode Vacuum;
    public KeyCode Pause;
    public KeyCode Manual;

    // Start is called before the first frame update
    void Start()
    {
        ControlsScript = this; // Allows us to reference this script in other scripts.
    }
}

