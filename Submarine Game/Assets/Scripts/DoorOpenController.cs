using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim = null;

    private bool doorOpen = false;

    [SerializeField] private string openAnimationName = "DoorOpen";
    [SerializeField] private string closeAnimationName = "DoorClose";

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    private IEnumerator pauseDoorInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void PlayAnimation()
    {
        if (!doorOpen && !pauseInteraction)
        {
            EnemySpawner.enemySpawner.IsSpawning = true;
            doorAnim.Play(openAnimationName, 0, 0.0f);
            doorOpen = true;
            StartCoroutine(pauseDoorInteraction());
        }

        //else if (doorOpen && pauseInteraction)
        //{
        //    doorAnim.Play(closeAnimationName, 0, 0.0f);
        //    doorOpen = false;
        //    StartCoroutine(pauseDoorInteraction());
        //}
    }
}
