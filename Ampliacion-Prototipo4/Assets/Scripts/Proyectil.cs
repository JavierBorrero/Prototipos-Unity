using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{

    private float speed = 10f;

    void Start()
    {
        // Se busca un GameObject con el tag Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        /*
         *  Se ignorar las colisiones con el player por parte de proyectil
         *  Cuando se instancian los proyectiles la posicion inicial que tienen es el transform.position de player
         *  Se chocan con el jugador y hace que rebote de una forma extraña, por eso el IgnoreCollisions
         */
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        // Mover el proyectil
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        // Destruir cuando se sale de unos limites
        if(transform.position.x > 15 || transform.position.x < -15 || transform.position.z > 15 || transform.position.z < -15)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si se impacta con un enemigo
        if(other.gameObject.CompareTag("Enemy"))
        {
            // Destruir el enemigo y el proyectil
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
