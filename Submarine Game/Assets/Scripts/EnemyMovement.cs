using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement : MonoBehaviour
{
    public Transform targetposition;
    public float direction;
    public bool target;
    public float enemydelay;
    public Vector3 targetpos;

    public Vector3 MinPosition;
    public Vector3 MaxPosition;
    public Vector3 RandomPosition;
    public Rigidbody rb;
    void Start()
    {
        target = true;
    }

  
    void Update()
    {
        move();
       // Charging();
    }
    private void move()
    {
        

        float StoppingDistance = Vector3.Distance(transform.position, targetposition.position);
        
        
            if (target)
             
            {
                
                Vector3 targetdirection = targetposition.position - transform.position;
                Quaternion targetrotation = Quaternion.LookRotation(targetdirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, Time.deltaTime);

                targetpos = targetposition.position;
                transform.DOMove(targetpos, direction).SetSpeedBased(true);
                
                }
            else
            {
            if (StoppingDistance < 0.5f)
            {
                RandomPosition = GetRandomPosition();

            }
            

            targetpos = RandomPosition;
            Vector3 targetdirection = targetpos - transform.position;
            Quaternion targetrotation = Quaternion.LookRotation(targetdirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, Time.deltaTime);
            transform.DOMove(targetpos, direction).SetSpeedBased(true);
            enemydelay -= Time.deltaTime;
            if (enemydelay <= 0)
            {
                target = true;
                enemydelay = 7;
            }
            }
        
      

            }
    public void Charging()
    {
        float StoppingDistance = Vector3.Distance(transform.position, targetposition.position);
        if (target)
        {
            Vector3 targetdirection =targetposition.position - transform.position;
            Quaternion targetrotation = Quaternion.LookRotation(targetdirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, Time.deltaTime);
            targetpos = targetposition.position;
            rb.AddForce(targetdirection * direction, ForceMode.Impulse);
        }
        else
        {
            if (StoppingDistance < 0.5f)
            {
                RandomPosition = GetRandomPosition();

            }

            targetpos = RandomPosition;
            transform.DOMove(targetpos, direction).SetSpeedBased(true);
            enemydelay -= Time.deltaTime;
            if (enemydelay <= 0)
            {
                target = true;
                enemydelay = 7;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = false;
             RandomPosition = GetRandomPosition();
        }
    }
    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(MinPosition.x, MaxPosition.x);
        float randomY = Random.Range(MinPosition.y, MaxPosition.y);
        float randomZ = Random.Range(MinPosition.z, MaxPosition.z);
        return new Vector3(randomX, randomY, randomZ);
    }
}
