using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

        private static GameObject[] persistentObjects = new GameObject[3];
        public int objectIndex;

        void Awake()
        {
            if (persistentObjects[objectIndex] == null)
            {
                persistentObjects[objectIndex] = gameObject;
                DontDestroyOnLoad(gameObject);
            }
            else if (persistentObjects[objectIndex].gameObject != gameObject)
            {
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Bosque")
            {
                GameObject objetos = GameObject.Find("Objetos");

                if (objetos != null)
                {
                    foreach (Transform child in objetos.transform)
                    {
                        if (!child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }