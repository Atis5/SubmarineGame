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
    List<GameObject> RadarObjects;
    List<GameObject> BorderObjects;


    private void Start()
    {
        TrashObjects = GameObject.FindGameObjectsWithTag("Pickable");
        CreateRadarObjects();
    }
    private void Update()
    {
        RadarCameraFollowPlayer();

        for (int i = 0; i < RadarObjects.Count; i++)
        {
            if (Vector3.Distance(RadarObjects[i].transform.position, transform.position) > BorderDistance)
            {
                // Switch to BorderObjects.
                RadarTransform.LookAt(RadarObjects[i].transform);
                BorderObjects[i].transform.position = transform.position + BorderDistance * RadarTransform.forward;
                BorderObjects[i].layer = LayerMask.NameToLayer("Radar");
                RadarObjects[i].layer = LayerMask.NameToLayer("Invisible"); //delete this code, come back with a degree in phsycology so you know how to mentally survive this turmoil of a project
            }
            else
            {
                // Switch to RadarObjects.
                BorderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                RadarObjects[i].layer = LayerMask.NameToLayer("Radar");
            }
        }
    }

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

    void RadarCameraFollowPlayer()
    {
        RadarCamera.transform.position = new Vector3(transform.position.x, transform.position.y+RadarDistance, transform.position.z);
        RadarCamera.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}