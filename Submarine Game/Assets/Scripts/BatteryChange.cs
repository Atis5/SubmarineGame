using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryChange : MonoBehaviour
{
    [SerializeField] private GameObject hundred;
    [SerializeField] private GameObject eighty;
    [SerializeField] private GameObject sixty;
    [SerializeField] private GameObject forty;
    [SerializeField] private GameObject twenty;

    void start()
    {
        hundred.SetActive(true);
    }


    void Update()
    {
        HpChange();
    }

    void HpChange()
    {
        if ((SubControl.SubControlScript.CurrentHealth <= 240))
        {
            hundred.SetActive(false);
            eighty.SetActive(true);
        }

        if ((SubControl.SubControlScript.CurrentHealth <= 180))
        {
            eighty.SetActive(false);
            sixty.SetActive(true);
        }

        if ((SubControl.SubControlScript.CurrentHealth <= 120))
        {
            sixty.SetActive(false);
            forty.SetActive(true);
        }

        if ((SubControl.SubControlScript.CurrentHealth <= 60))
        {
            forty.SetActive(false);
            twenty.SetActive(true);
        }
    }
}
