using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public int animalIndex;
    private float spawnRange = 20;
    private float startDelay = 2;
    private float spawnInterval = 1.5f;

    // Spawn position
    private float spawnPositionRight = 20;
    private float spawnPositionLeft = -20;
    private float spawnPositionTop = 20;

    void Start()
    {
        InvokeRepeating("spawnRandomAnimal", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        //    int animalIndex = Random.Range(0, animalPrefabs.Length);
        //    Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[animalIndex].transform.rotation);
        //}
    }

    void spawnRandomAnimal()
    {
        int randNum = Random.Range(0, 3);
        int animalIndex = Random.Range(0, animalPrefabs.Length);

        switch (randNum)
        {
            case 0:
                spawnAnimalLeft(animalIndex);
                break;

            case 1:
                spawnAnimalTop(animalIndex);
                break;

            case 2:
                spawnAnimalRight(animalIndex);
                break;

        }
    }

    void spawnAnimalTop(int animalIndex)
    {
        // Spawn position desde la lado superior
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, spawnPositionTop);
        Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[animalIndex].transform.rotation);
    }

    void spawnAnimalLeft(int animalIndex)
    {   
        // Spawn Position desde el lado izquierdo
        Vector3 spawnPos = new Vector3(spawnPositionLeft, 0, Random.Range(-spawnRange, spawnRange));
        // Rotar el animal en el spawn
        Instantiate(animalPrefabs[animalIndex], spawnPos, Quaternion.Euler(0,90,0));
    }

    void spawnAnimalRight(int animalIndex)
    {
        // Spawn position desde el lado derecho
        Vector3 spawnPos = new Vector3(spawnPositionRight, 0, Random.Range(-spawnRange, spawnRange));
        // Rotar animal en el spawn
        Instantiate(animalPrefabs[animalIndex], spawnPos, Quaternion.Euler(0, 270, 0));
    }
}
