using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager1 : MonoBehaviour
{
    public static SceneTransitionManager1 Instance;

    private Vector3 playerPosition;
    private string lastScene;
    private string enemyToRemove;
    private GameObject inventoryCanvas; // Para almacenar el canvas de inventario
    private GameObject inventoryPrefab; // Para almacenar el prefab de inventario

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetInventory(GameObject Inve, GameObject Inventario)
    {
        inventoryCanvas = Inve;
        inventoryPrefab = Inventario;
        DontDestroyOnLoad(inventoryCanvas);  // Evita que el canvas se destruya al cargar la escena
        DontDestroyOnLoad(inventoryPrefab);  // Evita que el prefab se destruya al cargar la escena
    }

    public void TransportToBattle(Vector3 position, string currentScene, string enemyName)
    {
        playerPosition = position;
        lastScene = currentScene;
        enemyToRemove = enemyName;
        SceneManager.LoadScene("TBC 1", LoadSceneMode.Single);
    }

    public void ReturnFromBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lastScene)
        {
            // Reposiciona al jugador
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = playerPosition;

            // Elimina al enemigo
            GameObject enemy = GameObject.Find(enemyToRemove);
            if (enemy != null)
            {
                Destroy(enemy);
            }

            // Reposiciona a los héroes con separación
            RepositionHeroes();
        }
        else if (scene.name == "TBC 1")
        {
            RestoreInventory();
        }
    }
    private void RestoreInventory()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(true);  // Restaurar el canvas de inventario si es necesario
        }

        if (inventoryPrefab != null)
        {
            Instantiate(inventoryPrefab);  // Restaurar el inventario si es necesario
        }
    }


    private void RepositionHeroes()
    {
        float separation = 1.5f; // Ajusta este valor para cambiar la distancia entre los héroes

        GameObject[] heroes = new GameObject[4];
        heroes[0] = GameObject.Find("heroe4");
        heroes[1] = GameObject.Find("heroe3");
        heroes[2] = GameObject.Find("heroe1");
        heroes[3] = GameObject.Find("heroe2");

        Vector3 startPosition = new Vector3(playerPosition.x - 3f, playerPosition.y, playerPosition.z); // Ajusta la posición inicial según necesites

        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null)
            {
                heroes[i].transform.position = startPosition + new Vector3(i * separation, 0, 0); // Desplaza horizontalmente a cada héroe
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}