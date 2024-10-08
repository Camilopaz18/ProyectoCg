using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    [SerializeField] private float cantidadCofres;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ControladorCofres.Instance.SumarCofres(cantidadCofres);
            Destroy(gameObject); 
        }
        
    }

}
