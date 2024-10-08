using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaManager : MonoBehaviour
{
    public static EscenaManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);

        }
        else
        {
            instance = this;
        }
    }
    public void inicioJuego()
    {
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }

    public void finJuego()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
