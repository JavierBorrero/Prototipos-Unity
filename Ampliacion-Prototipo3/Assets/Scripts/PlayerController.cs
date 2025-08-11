using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    // Doble salto
    public bool canDoubleJump = false;

    // Puntuacion
    public int puntuacion;
    public bool doublePoints = false;

    // Aparecer personaje desde la izquierda de la pantalla
    private Vector3 targetPosition = new Vector3(0,0,0);
    private float animSpeed = 3f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        puntuacion = 0;

        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.x != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animSpeed * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround && !gameOver)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = true;
                isOnGround = false;
                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
            }
            else if(canDoubleJump)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            MoveLeft.speed = 15;
            doublePoints = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MoveLeft.speed = 10;
            doublePoints = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
        }
        else if (collision.gameObject.CompareTag("Puntuacion"))
        {
            if(doublePoints)
            {
                puntuacion = puntuacion + 2;
                Debug.Log(puntuacion);
            }
            else
            {
                puntuacion = puntuacion + 1;
                Debug.Log(puntuacion);
            }
        }
    }
}
