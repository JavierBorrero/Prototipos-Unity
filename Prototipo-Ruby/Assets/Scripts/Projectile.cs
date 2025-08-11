using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController ec = other.collider.GetComponent<EnemyController>();

        if(ec != null)
        {
            ec.Fix();
        }
        /*
         *  He añadido este else if porque si disparas proyectiles muy rapido se juntan 
         *  unos con los otros y se destruyen, entonces parece que no se estan disparando
         */
        else if (other.collider.GetComponent<Projectile>()) return;

        Destroy(gameObject);

    }
}
