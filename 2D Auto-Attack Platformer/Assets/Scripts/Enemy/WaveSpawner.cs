using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    private float coolDown;
    private float timeSinceLastSpawn;
    private GameObject nextSpawnPoint;
    private System.Random rnd = new System.Random();
    public GameObject[] enemyPrefab;
    private GameObject enemySpawned;

    public float difficultyIncreaseTimer;
    public float difficultyIncreaseLength;
    public int waveNumber;
    private int activeSpawnPoints;
    private float enemyHealthMultiplier;
    private void Start()
    {
        enemyHealthMultiplier = 0f;
        waveNumber = 0;
        difficultyIncreaseLength = 45f;
        coolDown = 1f;
        timeSinceLastSpawn = 1f;
        activeSpawnPoints = 1;
    }

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        difficultyIncreaseTimer += Time.deltaTime;
        if ( timeSinceLastSpawn > coolDown )
        {
            //loop through pick a randoms spawn point equal to activeSpawnPoints
            for (int x = 0; x < activeSpawnPoints; x++)
            {
                nextSpawnPoint = spawnPoints[rnd.Next(4)];
                // Instantiate a new enemy
                enemySpawned = GameObject.Instantiate(enemyPrefab[rnd.Next(2)], nextSpawnPoint.transform.position, nextSpawnPoint.transform.rotation);
                enemySpawned.GetComponent<HealthSystem>().increaseHealth(enemyHealthMultiplier);
            }
            timeSinceLastSpawn = 0f;
        }
        if (difficultyIncreaseTimer >= difficultyIncreaseLength)
        {
            increaseDifficulty();
        }
    }

    private void increaseDifficulty()
    {
        //Decrease the difficultyIncreaseLength to a lower time
        difficultyIncreaseLength -= .1f;
        //Increase the waveNumber
        waveNumber++;
        //Reset the difficultyIncreaseTimer 
        difficultyIncreaseTimer = 0;
        //if the wave length is divisibly by 3 enable another spawn point
        if (waveNumber % 3 == 0 && activeSpawnPoints < spawnPoints.Length)
        {
            activeSpawnPoints++;
        } else if (waveNumber % 2 == 0)
        {
            //increase health of spawned enemies
            enemyHealthMultiplier += .25f;
            print("Enemies Health Increased: " + enemyHealthMultiplier);
        }

    }
}
