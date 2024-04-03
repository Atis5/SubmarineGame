using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public GameObject Submarine;
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
    }


    void PickUpObject(GameObject PickableObject)
    {
        if (PickableObject.GetComponent<Rigidbody>())
        {
            HeldObject = PickableObject;
            HeldObjectRigidbody = PickableObject.GetComponent<Rigidbody>();
            HeldObjectRigidbody.isKinematic = true;
            HeldObjectRigidbody.transform.parent = HoldPosition.transform;
            Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), Submarine.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(HeldObject.GetComponent<Collider>(), Submarine.GetComponent<Collider>(), false);
        HeldObjectRigidbody.isKinematic = false;
        HeldObject.transform.parent = null;
        HeldObject = null;
        HeldObjectRigidbody.AddForce(transform.forward * 10);
    }
    void MoveObject()
    {
        HeldObject.transform.position = HoldPosition.transform.position;
    }

    void StopClipping()
    {
        var ClippingRange = Vector3.Distance(HeldObject.transform.position, transform.position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), ClippingRange);
        if (hits.Length > 1)
        {
            HeldObject.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}
