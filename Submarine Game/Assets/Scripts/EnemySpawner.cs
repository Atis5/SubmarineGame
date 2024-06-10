using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner enemySpawner;
    public GameObject EnemyPrefab;
    public bool IsSpawning;
    public List<Transform> SpawnPositions;
   
    public float SpawnDelay;
    void Start()
    {
        enemySpawner = this;
        //IsSpawning = true;
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
        int randomPos = Random.Range(0, SpawnPositions.Count);
        GameObject SpawnObj= Instantiate(EnemyPrefab);
        SpawnObj.transform.position = SpawnPositions[randomPos].transform.position;
       DefensesSystemPlayer.defensesSystemPlayer.enemies.Add(SpawnObj);
        IsSpawning = false;
    }
    
   
}
