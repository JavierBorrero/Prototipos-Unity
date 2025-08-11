using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public GameObject bossEnemyPrefab;
    private GameObject goSpawnManager;
    private SpawnManager spawnManager;
    public bool isGenerating;
    public int cantidadEnemigos;


    void Start()
    {
        goSpawnManager = GameObject.Find("SpawnManager");
        spawnManager = goSpawnManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGenerating)
        {
            StartCoroutine(GenerarEnemigos());
        }
    }

    IEnumerator GenerarEnemigos()
    {
        isGenerating = true;

        for(int i = 0; i< cantidadEnemigos; i++)
        {
            GameObject go = Instantiate(bossEnemyPrefab, spawnManager.GenerateSpawnPosition(), bossEnemyPrefab.transform.rotation);
            spawnManager.listaEnemigos.Add(go);
            spawnManager.rbEnemigos.Add(go.GetComponent<Rigidbody>());
        }

        yield return new WaitForSeconds(5f);

        isGenerating = false;
    }
}
