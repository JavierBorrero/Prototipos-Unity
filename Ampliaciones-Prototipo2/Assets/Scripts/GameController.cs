using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int vidas = 3;
    public int puntuacion = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Vidas: 3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        puntuacion++;
        Debug.Log("Puntuacion: " + puntuacion);
    }

    public void RemoveHP()
    {
        vidas--;

        if(vidas > 0)
        {
            Debug.Log("Vidas: " + vidas);
        }
        if(vidas <= 0)
        {
            Debug.Log("GameOver");
        }
    }
}
