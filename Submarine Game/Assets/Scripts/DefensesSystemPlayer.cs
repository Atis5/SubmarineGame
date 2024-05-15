using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DefensesSystemPlayer : MonoBehaviour
{
    public EnemyMovement[] enemies;
    public Image WarningScreen;
    void Start()
    {
        
    }


    void Update()
    {
        Defense();
    }
    public void Defense()
    {
        foreach(var target in enemies)
        {
                EnemyMovement obj = target.GetComponent<EnemyMovement>();
            if (obj.target == true)
            {
                WarningScreen.enabled = true;
                if (Input.GetKeyDown(Controls.ControlsScript.Defense))
                {
                    obj.target = false;
                }
            }
            else
            {
                WarningScreen.enabled = false;
            }
         
        }
        
    }
}
