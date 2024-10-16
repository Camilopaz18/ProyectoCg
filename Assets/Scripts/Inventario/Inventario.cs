using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public List<GameObject> Bag = new List<GameObject>();
    public GameObject inv;
    public bool Activar_inv;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Cofre"))
        {
            try
            {
                for (int i = 0; i < Bag.Count; i++)
                {
                    // Verifica si el objeto no es nulo antes de acceder a sus componentes
                    if (Bag[i] != null && Bag[i].GetComponent<Image>() != null && Bag[i].GetComponent<Image>().enabled == false)
                    {
                        Bag[i].GetComponent<Image>().enabled = true;
                        Bag[i].GetComponent<Image>().sprite = coll.GetComponent<SpriteRenderer>().sprite;
                        break;
                    }
                }
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning("Referencia a objeto destruido: " + e.Message);
            }
        }
    }

    void Start()
    {
        // Inicialización si es necesario
    }

    void Update()
    {
        // Limpia la lista de objetos destruidos
        Bag.RemoveAll(item => item == null);

        if (Activar_inv)
        {
            inv.SetActive(true);
        }
        else
        {
            inv.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Activar_inv = !Activar_inv;
        }
    }
}
