using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public GameObject[] waypoints; 
    //public Vector3[] spawnPoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetRandomXZCoordinate()
    {
        if (waypoints.Length > 0)
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            GameObject randomWaypoint = waypoints[randomIndex];
            if (randomWaypoint != null)
            {
                return randomWaypoint.transform.position;
            }
            else
            {
                Debug.LogError("Random waypoint is null.");
                return Vector3.zero; 
            }
        }
        else
        {
            Debug.LogError("No spawn waypoints defined.");
            return Vector3.zero; 
        }
    }

    /*public Vector3 GetRandomXZCoordinate()
    {
        if (spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 randomSpawnPoint = spawnPoints[randomIndex];
            return new Vector3(randomSpawnPoint.x, 0f, randomSpawnPoint.z);
        }
        else
        {
            Debug.LogError("No spawn points defined.");
            return Vector3.zero; 
        }
    }*/
}
