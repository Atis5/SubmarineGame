using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMovement : MonoBehaviour
{
    public bool dies;
    public Transform Playerposition;
    public float direction;
    public bool target;
    public float enemydelay;
    public Vector3 targetpos;

    public Vector3 MinPosition;
    public Vector3 MaxPosition;
    public Vector3 RandomPosition;
    public Rigidbody rb;
    public float MaxSpeed;
    void Start()
    {
      GameObject playerPos = GameObject.Find("TargetSpot");
        Playerposition = playerPos.transform;
        target = true;
    }


    void Update()
    {
        DestroyEnemy();
        Charging();
    }
    public void Charging()
    {
        if (target)
        {
            targetpos = Playerposition.position;
            Vector3 pos = (targetpos - transform.position).normalized;
            rb.AddForce(pos * direction);
            if (rb.velocity.magnitude > MaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * MaxSpeed;
            }
        }
        else
        {
            targetpos = RandomPosition;
            Vector3 randomPos = (targetpos - transform.position).normalized;
            rb.AddForce(randomPos * direction);
            if (rb.velocity.magnitude > MaxSpeed/2f)
            {
                rb.velocity = rb.velocity.normalized * (MaxSpeed/2f);
            }
            enemydelay -= Time.deltaTime;
            if (enemydelay <= 0)
            {
                rb.velocity = Vector3.zero;
                target = true;
                enemydelay = 7;
            }
        }
    }
    private void DestroyEnemy()
    {
        if (dies)
        {
            // DefensesSystemPlayer.defensesSystemPlayer.WarningScreen.enabled = false;
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RandomPos());
            target = false;
            Debug.Log("abc");
        }
    }
    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(MinPosition.x, MaxPosition.x);
        float randomY = Random.Range(MinPosition.y, MaxPosition.y);
        float randomZ = Random.Range(MinPosition.z, MaxPosition.z);
        return new Vector3(randomX, randomY, randomZ);
    }
    IEnumerator RandomPos()
    {
        yield return new WaitForSeconds(0f);
        target = false;
        RandomPosition = GetRandomPosition();
        Debug.Log("Random");
    }
}
