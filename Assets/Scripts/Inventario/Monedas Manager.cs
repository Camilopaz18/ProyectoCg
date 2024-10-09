using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MonedasManager : MonoBehaviour
{
    public GameObject targetCanvas;
    public Text contador;
    public int numMonedas;

    // Referencias a los objetos del canvas
    public RectTransform imagen1;
    public RectTransform imagen2;
    public RectTransform titulo;
    public RectTransform texto;

    private bool isActive = true;

    // Tamaños y posiciones originales
    private Vector2 imagen1OriginalSize;
    private Vector3 imagen1OriginalPos;
    private Vector2 imagen2OriginalSize;
    private Vector3 imagen2OriginalPos;
    private Vector2 tituloOriginalSize;
    private Vector3 tituloOriginalPos;
    private Vector2 textoOriginalSize;
    private Vector3 textoOriginalPos;

    void Awake()
    {
        // Guardamos las posiciones y tamaños originales al iniciar
        imagen1OriginalSize = imagen1.sizeDelta;
        imagen1OriginalPos = imagen1.localPosition;

        imagen2OriginalSize = imagen2.sizeDelta;
        imagen2OriginalPos = imagen2.localPosition;

        tituloOriginalSize = titulo.sizeDelta;
        tituloOriginalPos = titulo.localPosition;

        textoOriginalSize = texto.sizeDelta;
        textoOriginalPos = texto.localPosition;
    }

    void Update()
    {
        // Si estamos en la escena "TBC" y el canvas aún está activo
        if (SceneManager.GetActiveScene().name == "TBC" && isActive)
        {
            // Cambiar tamaño y posición al entrar a la escena "TBC"
            imagen1.sizeDelta = new Vector2(788, 287); // Ajusta estos valores según lo que necesites
            imagen1.localPosition = new Vector3(-827, -187, 0); // Ajusta estas posiciones

            imagen2.sizeDelta = new Vector2(100, 150);
            imagen2.localPosition = new Vector3(-905, -185, 0);

            titulo.sizeDelta = new Vector2(400, 200);
            titulo.localPosition = new Vector3(-830, -170, 0);

            texto.sizeDelta = new Vector2(510, 200);
            texto.localPosition = new Vector3(-830, -198, 0);

            isActive = false;
        }
        else if (SceneManager.GetActiveScene().name != "TBC" && !isActive)
        {
            // Restaurar las posiciones y tamaños originales al salir de la escena "TBC"
            imagen1.sizeDelta = imagen1OriginalSize;
            imagen1.localPosition = imagen1OriginalPos;

            imagen2.sizeDelta = imagen2OriginalSize;
            imagen2.localPosition = imagen2OriginalPos;

            titulo.sizeDelta = tituloOriginalSize;
            titulo.localPosition = tituloOriginalPos;

            texto.sizeDelta = textoOriginalSize;
            texto.localPosition = textoOriginalPos;

            isActive = true;
        }

        // Actualizar el contador de monedas
        contador.text = numMonedas.ToString();
    }

    public static void monedas(int _monedas)
    {
        // Agregar monedas y actualizar el contador
        MonedasManager current = FindObjectOfType<MonedasManager>();
        current.numMonedas += _monedas;
        current.contador.text = current.numMonedas.ToString();
    }
}

