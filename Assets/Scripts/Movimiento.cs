using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movimiento : MonoBehaviour
{

 public float Speed;
    public float JumpForce;
    public GameObject[] personajes; // Arreglo para almacenar los personajes
    public float distanciaSeguimiento = 2f; // Distancia a la que los personajes deben seguir al líder

    private Rigidbody2D rigidBody2d;
    private Animator animator;
    private float Horizontal;
    private bool Grounded;
    private Vector3 originalScale;
    private int Health = 50;



    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale; // Asigna la escala original
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Mismo código para el control de la dirección y animación
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);

        animator.SetBool("running", Horizontal != 0.0f);

        // Detección del suelo
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;


        // Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();

        }


        // Seguimiento de personajes
        if(personajes != null && personajes.Length >= 3)
{
            for (int i = 1; i < personajes.Length; i++)
            {
                GameObject personajeActual = personajes[i];
                GameObject personajeAnterior = personajes[i - 1];

                // Calcular la dirección hacia el personaje anterior
                Vector3 direccion = (personajeAnterior.transform.position - personajeActual.transform.position).normalized;

                // Mover el personaje actual una pequeña distancia en la dirección calculada
                personajeActual.transform.position += direccion * Speed * Time.deltaTime;
            }
        }
    }



    private void Jump()
    {
        rigidBody2d.AddForce(Vector2.up * JumpForce);
    }

    private void FixedUpdate()
    {
        rigidBody2d.velocity = new Vector2(Horizontal
 * Speed, rigidBody2d.velocity.y);
    }

    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0) Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D
 collision)
    {
        
    }
}

