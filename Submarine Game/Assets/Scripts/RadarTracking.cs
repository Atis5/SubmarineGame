using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTracking : MonoBehaviour
{
    [SerializeField] private GameObject RadarTrashPrefab;
    [SerializeField] private GameObject[] TrashObjects;
    List<GameObject> RadarObjects;


    private void Start()
    {
        //TrashObjects = GameObject.FindGameObjectsWithTag("Trash Collectible");
    }
    private void Update()
    {
        
    }

    void CreateRadarObjects()
    {
        RadarObjects = new List<GameObject>();

        foreach (GameObject o in TrashObjects)
        {
            GameObject k = Instantiate(RadarTrashPrefab, o.transform.position, Quaternion.identity) as GameObject;
            RadarObjects.Add(k);
        }
    }
}
