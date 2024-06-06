using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTracking : MonoBehaviour
{
    [SerializeField] private float BorderDistance;
    [SerializeField] private float RadarDistance;
    [SerializeField] private GameObject RadarTrashPrefab;
    [SerializeField] private Transform RadarTransform;
    [SerializeField] private GameObject[] TrashObjects;
    [SerializeField] private GameObject RadarCamera;
    [SerializeField] private GameObject PlayerDot;
    List<GameObject> RadarObjects;
    List<GameObject> BorderObjects;



    private void Start()
    {
        // Puts all "Pickable" objects into an array.
        TrashObjects = GameObject.FindGameObjectsWithTag("Pickable");

        CreateRadarObjects();
    }



    private void Update()
    {
        RadarCameraFollowPlayer();
        SwitchObjectLayers();

        // Prevents middle dot to change size when player goes up or down.
        PlayerDot.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }



    /// <summary>
    /// Creates all objects from the array as objects in the scene. They are only visible to Radar Camera.
    /// </summary>
    void CreateRadarObjects()
    {
        RadarObjects = new List<GameObject>();
        BorderObjects = new List<GameObject>();

        foreach (GameObject o in TrashObjects)
        {
            GameObject RadarObjectInstance = Instantiate(RadarTrashPrefab, o.transform.position, Quaternion.identity) as GameObject;
            RadarObjects.Add(RadarObjectInstance);

            GameObject BorderObjectInstance = Instantiate(RadarTrashPrefab, o.transform.position, Quaternion.identity) as GameObject;
            BorderObjects.Add(BorderObjectInstance);
        }
    }



    /// <summary>
    /// Switches objects from Border to Radar space. 
    /// </summary>
    void SwitchObjectLayers()
    {
        for (int i = 0; i < RadarObjects.Count; i++)
        {
            if (Vector3.Distance(RadarObjects[i].transform.position, transform.position) > BorderDistance)
            {
                RadarTransform.LookAt(RadarObjects[i].transform);

                // BorderObjects follow player, but always stay on the same height.
                BorderObjects[i].transform.position = new Vector3(transform.position.x, 0, transform.position.z) + BorderDistance * RadarTransform.forward;

                // Switch to BorderObjects.
                BorderObjects[i].layer = LayerMask.NameToLayer("Radar");
                RadarObjects[i].layer = LayerMask.NameToLayer("Invisible"); //delete this code, come back with a degree in phsycology so you know how to mentally survive this turmoil of a project   <- Comment by Ryan
            }
            else
            {
                // Switch to RadarObjects.
                BorderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                RadarObjects[i].layer = LayerMask.NameToLayer("Radar");
            }
        }
    }

    /// <summary>
    /// Makes camera follows player's only horizontal position and rotation.
    /// </summary>
    void RadarCameraFollowPlayer()
    {
        RadarCamera.transform.position = new Vector3(transform.position.x, RadarCamera.transform.position.y, transform.position.z);
        RadarCamera.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}