using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    // spawner point array
    public GameObject[] spawners;    // spawn point array container

    // object to spawn
    public GameObject zombie;        // zombie
    public int spawnpoints = 3;      // how many spawn point

    // spawn timer 
    public float currentTime = 0f;   // current spawn timer
    public float startingTime = 5f;  // starting spawn timer

    // spawn checker
    private bool isSpawning = false;

    // invoke
    public PlayerGameManager PM;


    //----------------------------------------------------------------------------------------------//
    //----------------------------------------Founction---------------------------------------------//
    //----------------------------------------------------------------------------------------------//


    // Starting set up
    public void Start()
    {
        PM = GameObject.Find("GameManager").GetComponent<PlayerGameManager>();

        currentTime = startingTime;              // spawn timer initialize
        spawners = new GameObject[spawnpoints];  // create spawn points

        // set up spawn points
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update
    public void Update()
    {
        // if the player enters area 
        if(isSpawning == true)
        {
            currentTime -= 1 * Time.deltaTime;   // countdown to spawn zombie

            if (currentTime <= 0)
            {
                SpawnEnemy();
                currentTime = startingTime;
            }
        }
    }

    // spawn
    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);

        var child = Instantiate(zombie, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        child.transform.parent = GameObject.Find("ZombieManager").transform;

    }
    
    // enter trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Area entered");
            isSpawning = true;

        }
    }

    // exit trigger
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger exited");
            isSpawning = false;
        }

    }

}
