# Prototipo 2

Esta actividad es un juego de disparar a objetivos en movimiento para conseguir puntos. El personaje se mueve en horizontal y los objetivos vienen desde arriba hacia abajo.

Lo primero es añadir el `GameObject` del jugador y ponerle como componente el script `PlayerController`. Primero vamos a programar el movimiento y los límites del jugador para que no se pueda salir de la pantalla.

También tenemos que crear un *prefab* en la escena con el proyectil, y luego lo añadimos a la carpeta *Prefabs*. Recuerda asignar el prefab al script del jugador en el editor.

```
public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 10.0f;
    public float xRange = 25;
    public GameObject projectilePrefab;

    void Start(){}

    void Update()
    {
        // Coger el input del jugador y mover al personaje a izquierda o derecha
        // dependiendo del valor de horizontalInput (-1, 1)
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime. speed);
    
        // Evitar que el personaje se salga de los márgenes de la pantalla
        if(transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if(transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Lanzar el proyectil cuando el jugador pulsa la barra espaciadora
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }
}
```

Creamos el script para controlar el movimiento del proyectil y lo añadimos al prefab:

```
public class MoveForward : MonoBehaviour
{
    public float speed = 40;

    void Start() {}

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
```

Ahora vamos a convertir los objetivos en prefabs de forma que los vamos a poder instanciar igual que al proyectil:

- Rotamos los objetivos 180 grados para que estén frente al personaje
- Les añadimos el script `MoveForward` para darles movimiento y editamos su propiedad *velocidad*
- Finalmente los arrastramos al directorio *Prefabs*

Tenemos que destruir los *GameObjects* que se salen de los límites del juego ya que ocupan memoria y recursos. Para ello vamos a crear el script `DestroyOutOfBounds` y lo añadimos como componente del proyectil y de los objetivos:

```
public class DestroyOutOfBounds : MonoBehaviour
{
    private float topBound = 30;
    private float lowerBound = -10;

    void Start(){}

    void Update()
    {
        if(transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if(transform.position.z < lowerBound)
        {
            Destroy(gameObject);
        }
    }
}
```

En este punto vamos a programar que los objetivos aparezcan aleatoriamente:

- Creamos un objetivo vacío y lo llamamos `SpawnManager`.
- A continuación, creamos un script y lo llamamos `SpawnManager`, y lo añadimos al objeto vacío.

```
public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public int animalIndex;
    private float spawnRangeX = 20;
    private float spawnPosZ = 20;
    private float startDelay = 2;
    private float spawnInterval = 1.5;

    void Start()
    {
        // Repetimos el método de spawn segun el spawnInterval
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    void Update(){}

    void SpawnRandomAnimal()
    {
        // Escogemos un objetivo aleatorio del array
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        
        // Se crea un spawnPosition en el que se va a instanciar el prefab
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        
        // Instanciamos el objetivo
        Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[animalIndex].transform.rotation);
    }
}
```

En la ventana de inspección del objeto vacío debemos ver el script y la lista vacía correspondiente al array declarado. La llenamos con los *prefabs* de los objetivos que tenemos creados desde la ventana de proyecto. Tenemos que añadir `Box Colliders` a los *prefabs* de los objetivos para poder destruirlos con los proyectiles. Ajustamos el *collider* al volumen del GameObject y nos aseguramos de que está marcado el *checkbox* **IsTrigger**.

Repetimos este proceso para el proyectil. Además, a éste debemos añadirle un componente `RigidBody` y debemos descarmar el uso de la gravedad para que los proyectiles vuelen.

Una vez que tenemos los `Box Colliders` con sus *triggers* marcados debemos escribir un script para destruir los objetos con los impactos. Lo creamos con el nombre `DetectCollisions`:

```
public class detectCollisions : MonoBehaviour
{
    void Start(){}
    void Update() {}

    void OnTriggerEnter(Collider other)
    {

        // Cuando tengamos una colision (proyectil) destruimos este objeto (objetivo) 
        // y el proyectil con el que ha impactado
        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}
```


