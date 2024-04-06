using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float maxDistance;
    public GameObject lastObject;
    public Color lastObjectColor;
    public bool isInteracting = false;
    public Camera MainCamera;

    void Start()
    {

    }


    void Update()
    {
        interact();
    }
    void interact()
    {
        Vector3 screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = MainCamera.ScreenPointToRay(screenCentre);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, maxDistance))
        {
            if (!isInteracting) 
            { 
                lastObject = hit.collider.gameObject;
                lastObjectColor = lastObject.GetComponent<Renderer>().material.color;
                lastObject.GetComponent<Renderer>().material.color = Color.red;
                isInteracting = true;
             }

        }
        else
        {
            if (isInteracting)
            {
                lastObject.GetComponent<Renderer>().material.color = lastObjectColor;
                isInteracting = false; 

            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.magenta);
    }
}
