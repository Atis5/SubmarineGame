using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensesSystemPlayer : MonoBehaviour
{
    public EnemyMovement[] enemies;
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
                if (Input.GetKeyDown(Controls.ControlsScript.Defense))
                {
                    obj.target = false;
                }
               
                }
            Debug.Log(obj.target);
        }
        
    }
}
