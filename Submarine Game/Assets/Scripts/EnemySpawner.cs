using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner enemySpawner;
    public GameObject EnemyPrefab;
    public bool IsSpawning;
    public Vector3 MinPosition;
    public Vector3 MaxPosition;
    public Vector3 RandomPosition;
    public float SpawnDelay;
    void Start()
    {
        enemySpawner = this;
        IsSpawning = true;
    }

    
    void Update()
    {
        spawn();
    }
    void spawn()
    {
        if (IsSpawning)
        {
            StartCoroutine(EnemySpawn());
            IsSpawning = false;
        }
    }
    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(SpawnDelay);
        RandomPosition = GetRandomPosition();
        GameObject SpawnObj= Instantiate(EnemyPrefab);
        SpawnObj.transform.position = RandomPosition;
       DefensesSystemPlayer.defensesSystemPlayer.enemies.Add(SpawnObj);
        IsSpawning = false;
    }
    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(MinPosition.x, MaxPosition.x);
        float randomY = Random.Range(MinPosition.y, MaxPosition.y);
        float randomZ = Random.Range(MinPosition.z, MaxPosition.z);
        return new Vector3(randomX, randomY, randomZ);
    }
}
