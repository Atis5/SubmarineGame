using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarAnimation : MonoBehaviour
{

    [SerializeField] private Transform RadarCenter;
    [SerializeField] private float LineRotationSpeed;

    private void Update()
    {
        RadarCenter.eulerAngles -= new Vector3(0, 0, LineRotationSpeed * Time.deltaTime);
    }
}
