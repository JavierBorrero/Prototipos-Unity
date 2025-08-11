using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject powerupIndicator;
    public GameObject proyectilPrefab;
    private GameObject focalPoint;
    private Rigidbody playerRb;

    public SpawnManager spawnManager;

    public float speed = 5.0f;
    public float powerupStrength;
    private float valorRotacion = 0f;
    private float duracionMovimiento = 1f;
    private int cantidadProyectiles = 8;

    public Powerup powerupState = Powerup.noPowerup;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Powerup1") && powerupState == Powerup.noPowerup)
        {
            powerupState = Powerup.bouncePowerup;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if(other.CompareTag("Powerup2") && powerupState == Powerup.noPowerup)
        {
            // Se cambia el powerupState
            powerupState = Powerup.rocketPowerup;

            // Se destruye el powerup
            Destroy(other.gameObject);

            // Se llama al metodo para disparar los proyectiles
            DispararProyectiles();
        }

        if(other.CompareTag("Powerup3") && powerupState == Powerup.noPowerup)
        {
            // Se cambia el powerupState
            powerupState = Powerup.smashPowerup;

            // Se destruye el powerup
            Destroy(other.gameObject);

            // Se llama al metodo para el ataque aplastante
            /*
             *  flujo de ejecucion:
             *  1. Se obtienen la posicion inicial y final, junto con una lista de todos los enemigos
             *  2. Se ejecuta la corutina, se sube y se baja al player + activar y desactivar las kinematic de los enemigos
             *  3. en el momento del impacto con la isla, llamar al metodo para echar lejos a los enemigos
             */
            IniciarMovimiento();
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupState = Powerup.noPowerup;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && powerupState == Powerup.bouncePowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void DispararProyectiles()
    {
        // Se recorre un bucle segun la cantidad de proyectiles especificados
        for(int i = 0; i < cantidadProyectiles; i++)
        {
            // Cada vez que se recorre el bucle se instancia un GameObject con el prefab del proyectil
            GameObject go = Instantiate(proyectilPrefab);

            // Se le da una posicion y una rotacion
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.Euler(90, 0, valorRotacion);

            // Se suma 45 a la rotacion para que a la hora de instanciar el siguiente GameObject no aparezca en el mismo lugar
            valorRotacion += 45;
        }

        // Al finalizar el bucle cambiar el valor de powerupState
        powerupState = Powerup.noPowerup;
    }

    private void IniciarMovimiento()
    {
        // Posicion en la que se encuentra el player al recoger el powerup
        Vector3 posicionInicial = transform.position;

        // Subir 5 unidades en eje Y
        Vector3 posicionFinal = new Vector3(transform.position.x, 5f, transform.position.z);

        StartCoroutine(MoverEntrePuntos(posicionInicial, posicionFinal, spawnManager.listaEnemigos, spawnManager.rbEnemigos));
    }


    IEnumerator MoverEntrePuntos(Vector3 inicio, Vector3 final, List<GameObject> listaDeEnemigos, List<Rigidbody> listaRbEnemigo)
    {
        float tiempoTranscurrido = 0f;

        // Parar los Rigidbodys de los enemigos
        for (int i = 0; i < listaDeEnemigos.Count; i++)
        {
            if(listaRbEnemigo[i] != null)
            {
                listaRbEnemigo[i].isKinematic = true;
            }
        }

        while (tiempoTranscurrido < duracionMovimiento)
        {
            // Calcular la posicion intermedia entre el punto A y el B
            float porcentaje = tiempoTranscurrido / duracionMovimiento;

            // Mover el objeto suavemente entre los 2 puntos usando Lerp
            transform.position = Vector3.Lerp(inicio, final, porcentaje);

            // Incrementar el timepo transcurrido
            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        // Cambiar la velocidad del RB para que el jugador vuelva al suelo al terminar la subida
        playerRb.velocity = new Vector3(0f, playerRb.velocity.y * 2, 0f);

        // Volver a activar los Rigidbodys de los enemigos
        for(int i = 0; i < listaRbEnemigo.Count; i++)
        {
            if(listaRbEnemigo[i] != null)
            {
                listaRbEnemigo[i].isKinematic = false;
            }
        }

        // Aqui hacer el propio ataque
        AtaqueAplastante();

        powerupState = Powerup.noPowerup;
    }

    private void AtaqueAplastante()
    {
        // Por cada enemigo
        for (int i = 0; i < spawnManager.listaEnemigos.Count; i++)
        {
            if(spawnManager.listaEnemigos[i] != null)
            {
                // Distancia entre el enemigo y el jugador
                float dist = Vector3.Distance(transform.position, spawnManager.listaEnemigos[i].transform.position);

                // Normaliza la distancia entre un rango (5 a 15), donde 5 es la distancia minima y 15 es la distancia maxima
                float normalizedDist = Mathf.InverseLerp(5f, 15f, dist);

                // Calcula la fuerza proporcional a la distancia
                float forceAmount = Mathf.Lerp(30f, 10f, normalizedDist);

                // Aplica la fuerza en direccion contraria al jugador
                Rigidbody enemyRb = spawnManager.rbEnemigos[i];
                Vector3 awayFromPlayer = (spawnManager.listaEnemigos[i].transform.position - transform.position).normalized;
                enemyRb.AddForce(awayFromPlayer * forceAmount, ForceMode.Impulse);
            }
        }
    }

    public enum Powerup
    {
        noPowerup,
        bouncePowerup,
        rocketPowerup,
        smashPowerup,
    }
}
