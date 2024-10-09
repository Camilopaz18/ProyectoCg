using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemigoMov1 : MonoBehaviour
{
    // Start is called before the first frame update
    
     public string enemyName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Vector3 playerPosition = collision.transform.position;
                string currentScene = SceneManager.GetActiveScene().name;

                SceneTransitionManager1.Instance.TransportToBattle(playerPosition, currentScene, enemyName);
            }
        }
    }
