using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private PlayerController playerController;

    // Array de prefabs
    public GameObject[] obstaclePrefab;
    public GameObject puntuacionPrefab;

    // Spawn position de los obstaculos
    private Vector3 spawnPos = new Vector3(25, 0, 0);

    // Spawn position del prefab para la puntuacion
    private Vector3 puntuacionSpawnPos = new Vector3(27, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        if(playerController.gameOver == false)
        {
            int obstacleIndex = Random.Range(0, obstaclePrefab.Length);
            Instantiate(obstaclePrefab[obstacleIndex], spawnPos, obstaclePrefab[obstacleIndex].transform.rotation);
            Instantiate(puntuacionPrefab, puntuacionSpawnPos, puntuacionPrefab.transform.rotation);
        }
    }
}
