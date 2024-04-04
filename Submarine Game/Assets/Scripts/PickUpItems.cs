using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public GameObject Submarine;
    public GameObject SubmarineCollision;
    public GameObject TrashBin;
    public Transform TrashCollector;
    public Transform HoldPosition;
    public float PickUpRange;
    private GameObject HeldObject;
    private Rigidbody HeldObjectRigidbody;
    private bool AllowDropping = true;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (HeldObject == null)
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
            else
            {
                if (AllowDropping == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (HeldObject != null)
            {
                SecureTrash();
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
            HeldObjectRigidbody.transform.parent = HoldPosition.transform;
            Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), SubmarineCollision.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
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
        HeldObject.transform.position = new Vector3(-8.56f, 5f, 15.41f);
        HeldObjectRigidbody.useGravity = true;
        HeldObject = null;
    }
}
