using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpItems : MonoBehaviour
{
    [Header ("Properties")]
    [SerializeField] private float PickUpRange; // How far you need to be from an item to pick it up.
    [SerializeField] private float PickUpCooldown; // How much time needs to pass before player can drop object after picking it up.
    [SerializeField] private float GrabArmStrength; // How quickly objects will be pulled towards the player.
    [SerializeField] private Color MarkColor; // What color will the items turn when they are pickable.
    

    [Header ("References")]
    [SerializeField] private GameObject TrashCollected; // Text that shows how much trash was collected.
    [SerializeField] private GameObject TrashCounterObject; // Text that shows how much trash was collected.
    [SerializeField] private TextMeshProUGUI TrashCounterText; // Text that shows how much trash was collected.
    [SerializeField] private GameObject SubmarineCollision; // Reference to the collision of our player.
    [SerializeField] private Transform HoldPosition; // This is where items you pick up will be.
    [SerializeField] private GameObject TrashBin; // Not used right now.
    [SerializeField] private Transform TrashCollector; // Not used right now.
    [SerializeField] private Camera MainCamera; // 1st Person Camera reference
    private GameObject HeldObject; // References whatever object player is holding.
    private Rigidbody HeldObjectRigidbody; // Rigidbody reference.

    [Header ("Necessary Variables")]
    public static PickUpItems PickUpScript; // Allows us to reference this script in other scripts.
    public float CollectedTrashCount = 0;
    private bool IsInteracting = false; // Needed for MarkPickabkleObjects method
    private bool IsHolding = false;
    private GameObject LastObject; // Needed for MarkPickabkleObjects method
    private Color LastObjectColor; // Needed for MarkPickabkleObjects method
    private float CurrentGrabArmStrength;
    private float CurrentPickUpCooldown;



    private void Start()
    {
        PickUpScript = this;

        // Make text invisible.
        /*TrashCollected.CrossFadeAlpha(0.0f, 0.05f, false);
        TrashCounter.CrossFadeAlpha(0.0f, 0.05f, false);*/
    }



    void Update()
    {
        MarkPickableObjects();
        ObjectInteraction();

    }


    void FixedUpdate()
    {
        if (CurrentPickUpCooldown <= PickUpCooldown)
        {
            CurrentPickUpCooldown++;
        }
    }



    /// <summary>
    /// Checks if player is looking at the object and calls other methods to pick or drop it.
    /// </summary>
    void ObjectInteraction()
    {
        if ((IsHolding == true) && (Input.GetKeyDown(Controls.ControlsScript.PickUp)))
        {
            StopClipping();
            DropObject();
        }
        else if (Input.GetKey(Controls.ControlsScript.PickUp) && (IsHolding == false) && (CurrentPickUpCooldown >= PickUpCooldown))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, PickUpRange))
            {
                if (hit.transform.gameObject.tag == "Pickable")
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
        }
    }



    public void PickUpObject(GameObject PickableObject)
    {
        if (PickableObject.GetComponent<Rigidbody>())
        {
            HeldObject = PickableObject;
            HeldObjectRigidbody = PickableObject.GetComponent<Rigidbody>();
            CurrentGrabArmStrength += GrabArmStrength * Time.deltaTime;
            HeldObject.transform.position = Vector3.MoveTowards(HeldObject.transform.position, HoldPosition.transform.position, CurrentGrabArmStrength);

            // Stick object to the player's Hold Position when it's close enough.
            if (Vector3.Distance(HeldObject.transform.position, HoldPosition.transform.position) < 5)
            {
                HeldObject.transform.position = HoldPosition.transform.position;
                HeldObjectRigidbody.isKinematic = true;
                HeldObjectRigidbody.transform.parent = HoldPosition.transform;
                Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), true);
                IsHolding = true;
                CurrentGrabArmStrength = 0;

                // Teleport trash to the trash bin.
                Invoke("SecureTrash", 1);

                
            }

        }
    }



    public void DropObject()
    {
        Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), false);
        HeldObjectRigidbody.isKinematic = false;
        HeldObject.transform.parent = null;
        HeldObject = null;
        IsHolding = false;
        CurrentPickUpCooldown = 0;
    }


    /// <summary>
    /// Prevents recently held objects from clipping to the player model right after dropping them.
    /// </summary>
    void StopClipping()
    {
        var ClippingRange = Vector3.Distance(HeldObject.transform.position, transform.position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), ClippingRange);
        if (hits.Length > 1)
        {
            HeldObject.transform.position = transform.position + new Vector3(0f, -5f, 0f);
        }
    }



    /// <summary>
    /// Teleports trash items towards the trash bin.
    /// </summary>
    public void SecureTrash()
    {
        HeldObjectRigidbody.isKinematic = false;
        HeldObject.transform.position = TrashCollector.transform.position;
        HeldObjectRigidbody.useGravity = true;
        HeldObject = null;
        IsHolding = false;

        // Add +1 to collected trash.
        CollectedTrashCount++;
        TrashCounterText.text = CollectedTrashCount.ToString();

        // Enable text trash collecting text.
        TrashCollected.SetActive(true);
        TrashCounterObject.SetActive(true);
    }



    /// <summary>
    /// Creates a raycast from camera position and if the ray hits the object, it will change its color.
    /// </summary>
    void MarkPickableObjects()
    {
        Vector3 screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = MainCamera.ScreenPointToRay(screenCentre);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 1000, Color.magenta);
        if ((Physics.Raycast(ray, out hit, PickUpRange)) && (HeldObject == null))
        {
            if (!IsInteracting)
            {
                if (hit.transform.gameObject.tag == "Pickable")
                {
                    LastObject = hit.collider.gameObject;
                    LastObjectColor = LastObject.GetComponent<Renderer>().material.color;
                    LastObject.GetComponent<Renderer>().material.color = MarkColor;
                    IsInteracting = true;
                }
            }

        }
        else
        {
            if (IsInteracting)
            {
                LastObject.GetComponent<Renderer>().material.color = LastObjectColor;
                IsInteracting = false;

            }
        }
    }



}
