using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameObject bossPrefab;

    public List<GameObject> listaEnemigos;
    public List<Rigidbody> rbEnemigos;

    public int enemyCount;
    public int waveNumber = 1;
    private int powerupIndex;
    private int enemyIndex;
    private float spawnRange = 9;


    void Start()
    {
        powerupIndex = Random.Range(0, 3);
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefabs[powerupIndex], GenerateSpawnPosition(), powerupPrefabs[powerupIndex].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Se escoge aleatoriamente entre uno de los dos powerups
        powerupIndex = Random.Range(0, 3);

        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;

            if(waveNumber % 5 == 0)
            {
                Instantiate(powerupPrefabs[powerupIndex], GenerateSpawnPosition(), powerupPrefabs[powerupIndex].transform.rotation);
                SpawnBossWave();
            }
            else
            {
                Instantiate(powerupPrefabs[powerupIndex], GenerateSpawnPosition(), powerupPrefabs[powerupIndex].transform.rotation);
                SpawnEnemyWave(waveNumber);
            }

        }
    }

    public Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0.3f, spawnPosZ);
        return randomPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        // Limpiar la lista de los enemigos anteriores
        listaEnemigos.Clear();
        rbEnemigos.Clear();
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            // Se escoge aleatoriamente entre uno de los dos enemigos
            enemyIndex = Random.Range(0, 2);
            
            // Se crea el GameObject
            GameObject go = Instantiate(enemyPrefabs[enemyIndex], GenerateSpawnPosition(), enemyPrefabs[enemyIndex].transform.rotation);

            // Se añade el enemigo a la lista de enemigos y el Rb a la lista de Rbs
            listaEnemigos.Add(go);
            rbEnemigos.Add(go.GetComponent<Rigidbody>());
        }
    }

    void SpawnBossWave()
    {
        // Limpiar la lista de los enemigos anteriores
        listaEnemigos.Clear();
        rbEnemigos.Clear();

        // Spawn Boss Prefab
        GameObject go = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        listaEnemigos.Add(go);
        rbEnemigos.Add(go.GetComponent<Rigidbody>());
    }
}
