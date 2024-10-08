using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCofres : MonoBehaviour
{
    public static ControladorCofres Instance;

    [SerializeField] private float cantidadCofres;

    private void Awake()
    {
        if (ControladorCofres.Instance == null)
        {
            ControladorCofres.Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SumarCofres(float cofres)
    {
        cantidadCofres += cofres;
    }

}
