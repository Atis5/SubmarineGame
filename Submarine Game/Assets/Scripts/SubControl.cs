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
    [SerializeField] private float MaxForwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MaxBackwardSpeed; // Submarine will not go faster than that.
    [SerializeField] private float MinSpeed; // How slow can Submarine be before stopping.
    [SerializeField] private float TurnSpeed; // How fast the submarine goes left and right.
    [SerializeField] private float RiseSpeed;   // How fast the submarine goes up and down.
    [SerializeField] private float StabilizationSmoothing; // How fast the submarine will stabilize after being rotated.
    [SerializeField] private float BumpForce; // How far the submarine will be pushed away after hitting something.
    [SerializeField] private float MaxHealth; // Player starts with this health and cannot heal above it.

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI SubmarineSpeedText;
    [SerializeField] private TextMeshProUGUI SubmarineDepthText;
    [SerializeField] private TextMeshProUGUI ToxicityNumberText;
    [SerializeField] private TextMeshProUGUI HealthNumberText;

    [Header("Necessary Variables")]
    public static SubControl SubControlScript; // Variable where the script reference is stored.
    public GameObject Player;
    public GameObject ToxicityWarning;
    public Collider SubmarineCollision; // Needed to make colliders work.
    private Rigidbody rb; // Reference to Rigidbody.
    private bool ToxicityWarningIsActive = false;  // Checks if the method is running.
    public bool IsInToxicArea = false;
    public  float SubmarineSpeed = 0; // Influenced by Acceleration.
    private float SubmarineDepth;
    private float ToxicityNumber = 10;
    private float CurrentBumpForce; // Influenced by BumpForce
    public float CurrentHealth;



    void Start()
    {
        // Lock Player's cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // References:
        SubControlScript = this; // Allows to reference this script in other scripts.
        rb = GetComponent<Rigidbody>(); // References Rigidbody component.

        // Set default values:
        CurrentHealth = MaxHealth;
        SubmarineDepth = Player.transform.position.y;

        // Update HUD at the start:
        SubmarineDepthText.text = SubmarineDepth.ToString() + "m";
        SubmarineSpeedText.text = SubmarineSpeed.ToString();
        ToxicityNumberText.text = ToxicityNumber.ToString() + " TU";
        HealthNumberText.text = CurrentHealth.ToString();
    }

    private void Update()
    {
        rb.AddTorque(transform.up * Input.GetAxis("Mouse X") * TurnSpeed/3);
        rb.AddTorque(transform.right * -Input.GetAxis("Mouse Y") * TurnSpeed/3);
    }

    void FixedUpdate()
    {

        // Movement
        ForwardAndBackwardMovement();
        Turning();
        RisingAndSinking();



        //HUD
        ShowToxicity();
        ShowDepth();
        MaxAndMinHealth();

        // Adding toxicity for debugging purposes.
        if (Input.GetKey(KeyCode.X))
        {
            ToxicityNumber += 10;
        }

    }



    private void ForwardAndBackwardMovement()
    {
        // Submarine speed slowly increases by the amount of acceleration.
        if (Input.GetKey(Controls.ControlsScript.Forward))
        {
            SubmarineSpeed += Acceleration;
            if (SubmarineSpeed < 0)
            {
                SubmarineSpeed++;
            }
        }
        else if (Input.GetKey(Controls.ControlsScript.Backward))
        {
            SubmarineSpeed -= Acceleration;
            if (SubmarineSpeed > 0)
                {
                    SubmarineSpeed--;
                }
        }
        else if (SubmarineSpeed > 0)
        {
            SubmarineSpeed--;
        }
        else if (SubmarineSpeed < 0)
        {
            SubmarineSpeed++;
        }

        // Stops submarine if speed is less than minimum. This is to prevent Submarine from moving very slowly and annoying players.
        // No longer needed due to changes in how the Submarine moves.
        /*else if (Mathf.Abs(SubmarineSpeed) <= MinSpeed)
        {
            SubmarineSpeed = 0;
        }
        */
        
        SubmarineSpeed = Mathf.Clamp(SubmarineSpeed, -MaxBackwardSpeed, MaxForwardSpeed);
        SubmarineSpeedText.text = Mathf.Round(SubmarineSpeed / 10).ToString() + " knots";
        rb.AddForce(transform.forward * SubmarineSpeed);
    }



    private void Turning()
    {
        if (Input.GetKey(Controls.ControlsScript.TurnUp))
        {
            rb.AddTorque(transform.right * -TurnSpeed);
        }
        else if (Input.GetKey(Controls.ControlsScript.TurnDown))
        {
            rb.AddTorque(transform.right * TurnSpeed);
        }
        
        if (Input.GetKey(Controls.ControlsScript.TurnRight))
        {
            rb.AddTorque(transform.up * TurnSpeed);
        }
        else if (Input.GetKey(Controls.ControlsScript.TurnLeft))
        {
            rb.AddTorque(transform.up * -TurnSpeed);

        }



        // Submarine stabilization
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(new Vector3(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, 0)), StabilizationSmoothing));
    }



    private void RisingAndSinking()
    {
        if ((Input.GetKey(Controls.ControlsScript.Rise)) || Input.GetMouseButton(0))
        {
            rb.AddForce(transform.up * RiseSpeed);
        }
        else if ((Input.GetKey(Controls.ControlsScript.Sink)) || Input.GetMouseButton(1))
        {
            rb.AddForce(transform.up * -RiseSpeed);
        }
    }


    private void ShowToxicity()
    {
        if ((IsInToxicArea == true) && (ToxicityNumber < 100))
        {
            ToxicityNumber += 0.2f;
            // Show toxicity meter
            ToxicityNumberText.text = Mathf.Round(ToxicityNumber).ToString() + " TU";
        }
        else if ((IsInToxicArea == false) && (ToxicityNumber > 10))
        {
            ToxicityNumber -= 0.2f;
            ToxicityNumberText.text = Mathf.Round(ToxicityNumber).ToString() + " TU";
        }

        if (ToxicityNumber >= 100)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth -= 0.05f;
                HealthNumberText.text = Mathf.Round(CurrentHealth).ToString();

            }
            if (ToxicityWarningIsActive == false)
            {
                StartCoroutine(ShowToxicityBlink());
                ToxicityWarningIsActive = true;
            }
        }
        
    }

    /*private void ShowToxicityWarning()
    {
            ToxicityWarning.SetActive(ToxicityWarningIsActive);
            ToxicityWarningIsActive = !ToxicityWarningIsActive;

    }*/

    private IEnumerator ShowToxicityBlink()
    {
        Debug.Log("Toxicity Warning!");
        ToxicityWarning.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ToxicityWarning.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        ToxicityWarningIsActive = false;
    }

    private void ShowDepth()
    {
        // Show submarine depth
        SubmarineDepth = Player.transform.position.y;
        SubmarineDepthText.text = Mathf.Round(SubmarineDepth - 1000).ToString() + " m";
    }


    private void MaxAndMinHealth()
        {
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

    // Bumping and tilting when hitting obstacles.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag != "Pickable"))
        {
            CurrentBumpForce = BumpForce * -SubmarineSpeed;
            CurrentHealth += CurrentBumpForce/100;
            HealthNumberText.text = Mathf.Round(CurrentHealth).ToString();
            SubmarineSpeed = 0;
            rb.AddForce(transform.forward * CurrentBumpForce);
            rb.AddTorque(transform.forward * -CurrentBumpForce);
            rb.AddTorque(transform.up * -CurrentBumpForce /2);
            CurrentBumpForce = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            IsInToxicArea = true;
        }
        if (other.gameObject.tag == "Enemy")
        {
            CurrentBumpForce = BumpForce * -SubmarineSpeed;
            CurrentHealth += CurrentBumpForce / 100;
            HealthNumberText.text = Mathf.Round(CurrentHealth).ToString();
            SubmarineSpeed = 0;
            rb.AddForce(transform.forward * CurrentBumpForce);
            rb.AddTorque(transform.forward * -CurrentBumpForce);
            rb.AddTorque(transform.up * -CurrentBumpForce / 2);
            CurrentBumpForce = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Toxic")
        {
            IsInToxicArea = false;
        }
    }

}
