using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemManager : MonoBehaviour
{
    public GameObject golem; // Reference to the golem object
    public Transform spawnPoint; // Location for the golem to appear

    private bool golemSpawned = false; // hides golem

    void Update()
    {
        // Filler name "TreeTracker" with example 3 trees being the trigger for him spawing
        if (TreeTracker.treesCut >= 3 && !golemSpawned)
        {
            SpawnGolem();
        }
    }

    void SpawnGolem()
    {
        golemSpawned = true; // Prevent further spawning
        golem.SetActive(true); // Show the golem
        golem.transform.position = spawnPoint.position; // Move to spawn point
    }
}