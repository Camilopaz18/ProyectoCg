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
            // Verifica si el objeto que colision� tiene el tag "Player"
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Colisi�n detectada con el jugador"); // Depuraci�n
                                                                // Cambia a la escena llamada "Bosque"
                SceneManager.LoadScene("Bosque");
            }
            else
            {
                Debug.Log("No es el jugador quien colision�: " + collision.name); // Depuraci�n
            }
        }
    }

}
