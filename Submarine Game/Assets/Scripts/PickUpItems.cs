using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [Header ("Properties")]
    public float PickUpRange; // How far you need to be from an item to pick it up.
    public Color MarkColor; // What color will the items turn when they are pickable.

    [Header ("References")]
    public GameObject SubmarineCollision; // Reference to the collision of our player.
    public Transform HoldPosition; // This is where items you pick up will be.
    private GameObject HeldObject; // References whatever object player is holding.
    private Rigidbody HeldObjectRigidbody; // Rigidbody reference.
    public GameObject TrashBin; // Not used right now.
    public Transform TrashCollector; // Not used right now.
    public Camera MainCamera;

    [Header ("Necessary Variables")]
    private bool AllowDropping = true; // If player holds anything, they can drop it. 
    public static PickUpItems PickUpScript;
    private bool IsInteracting = false;
    private GameObject LastObject;
    private Color LastObjectColor;



    private void Start()
    {
        PickUpScript = this;
    }

    void Update()
    {
        MarkPickableObjects();

        if (Input.GetKeyDown(SubControl.Main.PickUp))
        {
            RaycastHit hit;
            if (HeldObject == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, PickUpRange))
                {
                    if (hit.transform.gameObject.tag == "Pickable")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (AllowDropping == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }
    }


    void PickUpObject(GameObject PickableObject)
    {
        if (PickableObject.GetComponent<Rigidbody>())
        {
            HeldObject = PickableObject;
            HeldObjectRigidbody = PickableObject.GetComponent<Rigidbody>();
            HeldObjectRigidbody.isKinematic = true;
            HeldObject.transform.position = HoldPosition.transform.position;
            HeldObjectRigidbody.transform.parent = HoldPosition.transform;
            Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), true);
        }
    }

    public void DropObject()
    {
        Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), false);
        HeldObjectRigidbody.isKinematic = false;
        HeldObject.transform.parent = null;
        HeldObject = null;
    }

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

    public void SecureTrash()
    {
        Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), false);
        HeldObjectRigidbody.isKinematic = false;
        HeldObject.transform.position = TrashCollector.transform.position;
        HeldObjectRigidbody.useGravity = true;
        HeldObject = null;
    }

    void MarkPickableObjects()
    {
        Vector3 screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = MainCamera.ScreenPointToRay(screenCentre);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PickUpRange))
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
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.magenta);
    }

}
