using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSpawner : MonoBehaviour
{
    // spawner point array
    public GameObject[] spawners;    // spawn point array container

    // object to spawn
    public GameObject zombie;        // zombie
    public int spawnpoints = 4;          // how many spawn point

    // spawn timer 
    public float currentTime = 0f;   // current wave timer
    public float startingTime = 10f;  // starting spawn timer

    // Starting set up
    public void Start()
    {
        currentTime = startingTime;  // timer initialize

        spawners = new GameObject[spawnpoints]; // initialize spawn points

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        // initialize some zombie on the street when starting game
        for (int i=0; i < spawners.Length; i++)
        {
            var child = Instantiate(zombie, spawners[i].transform.position, spawners[i].transform.rotation);
            child.transform.parent = GameObject.Find("ZombieManager").transform;
        }

    }

    // Update is called once per frame
    public void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
        {
            SpawnEnemy();
            currentTime = startingTime;
        }
    }

    // spawn
    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);

        var child = Instantiate(zombie, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        child.transform.parent = GameObject.Find("ZombieManager").transform;

    }

}
