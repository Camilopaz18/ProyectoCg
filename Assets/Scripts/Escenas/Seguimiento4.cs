using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seguimiento5: MonoBehaviour
{
    [SerializeField] public Transform jugador;
    [SerializeField] private float distancia;

    public Vector3 puntoInicial;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
 // Guardar el punto de colisión

    private void Start()
    {
        animator = GetComponent<Animator>();
        puntoInicial = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (jugador != null) // Verificar si el jugador aún existe
        {
            distancia = Vector2.Distance(transform.position, jugador.position);
            animator.SetFloat("Distancia", distancia);
        }
    }


    public void Girar (Vector3 objetivo)
    {
        if(transform.position.x < objetivo.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Verifica si es el jugador
        {
            // Guardar el punto de colisión
            PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat("ContactPointX", transform.position.x);
            PlayerPrefs.SetFloat("ContactPointY", transform.position.y);

            // Guardar el sprite del enemigo
            PlayerPrefs.SetString("EnemySprite", spriteRenderer.sprite.name);

            // Cambiar la escena
            SceneManager.LoadScene("Final"); // Cargar la escena "TCB"

            // Destruir el objeto actual
           
        }
    }
}

