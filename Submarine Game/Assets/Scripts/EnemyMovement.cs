using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement : MonoBehaviour
{
    public Transform targetposition;
    public int direction;
    void Start()
    {
        
    }

  
    void Update()
    {
        move();
    }
    private void move()
    {
        Vector3 targetdirection = targetposition.position - transform.position;
        Quaternion targetrotation = Quaternion.LookRotation(targetdirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, Time.deltaTime);

        transform.DOMove(targetposition.position, direction)
            .SetEase(Ease.OutQuad);
            }
}
