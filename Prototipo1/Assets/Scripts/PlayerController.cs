using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inputs vertical y horizontal
    public float forwardInput;
    public float horizontalInput;

    // Velocidades
    public float velocidad = 20.0f;
    public float velocidadGiro = 45.0f;

    // Cameras
    public Camera mainCamera;
    public Camera conductorCamera;
    public KeyCode switchKey;

    public string inputID;

    void Start()
    {
    }

    void Update()
    {
        // Obtener Input de las flechas del teclado
        horizontalInput = Input.GetAxis("Horizontal" + inputID);
        forwardInput = Input.GetAxis("Vertical" + inputID);

        /*
         * Movimiento para horizontal y vertical
         * 
         * Linea comentada: El vehiculo solo se desliza y no gira
        */
        //transform.Translate( Vector3.right * Time.deltaTime * velocidadGiro * horizontalInput );

        transform.Translate( Vector3.forward * Time.deltaTime * velocidad * forwardInput );
        transform.Rotate( Vector3.up, velocidadGiro * horizontalInput * Time.deltaTime );

        if (Input.GetKeyDown(switchKey))
        {
            mainCamera.enabled = !mainCamera.enabled;
            conductorCamera.enabled = !conductorCamera.enabled;
        }
    }
}
