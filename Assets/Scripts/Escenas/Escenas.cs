using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escenas : MonoBehaviour
{
    public class SceneChanger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Verifica si el objeto que colisionó tiene el tag "Player"
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Colisión detectada con el jugador"); // Depuración
                                                                // Cambia a la escena llamada "Bosque"
                SceneManager.LoadScene("Bosque");
            }
            else
            {
                Debug.Log("No es el jugador quien colisionó: " + collision.name); // Depuración
            }
        }
    }

}
