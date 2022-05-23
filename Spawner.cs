using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    // spawner point array
    public GameObject[] spawners;    // spawn point array container

    // object to spawn
    public GameObject zombie;        // zombie
    public int spawnpoints = 4;          // how many spawn point

    // wave timer 
    public float currentTime = 0f;   // current wave timer
    public float startingTime = 7f;  // starting wave timer

    // wave
    public int waveNumber = 0;       // current wave number
    public int enemySpawnAmount = 0; // how many zombie spawn
    public int enemyKilled = 0;      // how many zombie got killed
    public int HardModeRound = 3;   // hard mode between each round 
    private bool HardMode = false;   // hard mode flag

    // Invoke
    public Animator animator;


    //----------------------------------------------------------------------------------------------//
    //----------------------------------------Founction---------------------------------------------//
    //----------------------------------------------------------------------------------------------//

    // starting set up
    public void Start()
    {
        animator = GameObject.Find("Directional Light").GetComponent<Animator>();

        currentTime = startingTime;    // wave timer initialize

        spawners = new GameObject[spawnpoints];  // create spawn points

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        StartWave();
    }

    public void Update()
    {
        if (enemyKilled >= enemySpawnAmount)
        {
            currentTime -= 1 * Time.deltaTime;

            if (currentTime <= 0)
            {
                NextWave();
                currentTime = startingTime;
            }
        }
    }

    // spawn
    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);

        Instantiate(zombie, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
    }

    // hard mode checker
    public void OnHardModeChecker()
    {
        if (waveNumber % HardModeRound == 0)
        {
            //Debug.Log("Hard Mode!!!!!");
            animator.SetBool("HardMode", true);
        }

        else
        {
            animator.SetBool("HardMode", false);
        }
    }

    // first wave
    private void StartWave()
    {
        waveNumber = 1;
        enemySpawnAmount = 1;
        enemyKilled = 0;

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }

    // next wave
    public void NextWave()
    {
        waveNumber++;
        enemySpawnAmount += 2; // add 2 more zombie per round
        enemyKilled = 0;

        OnHardModeChecker();

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }

    }

}
