using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DefensesSystemPlayer : MonoBehaviour
{
    public static DefensesSystemPlayer defensesSystemPlayer;
    public List<GameObject> enemies;
    public Image WarningScreen;
    void Start()
    {
        defensesSystemPlayer = this;
        WarningScreen.enabled = false;
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
            float AlertDistance = Vector3.Distance(transform.position, target.transform.position);
            Debug.Log(AlertDistance);
            if (AlertDistance<15)
            {
                WarningScreen.enabled = true;
                if (Input.GetKeyDown(Controls.ControlsScript.Defense))
                {
                    enemies.Remove(target);
                    obj.dies = true;
                    EnemySpawner.enemySpawner.IsSpawning = true;
                }

            }
            else
            {
                WarningScreen.enabled = false;
            }

        }
        
    }
}
