using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform heroe2;

    void Update()
    {
        if (heroe2 != null)
        {
            Vector3 position = transform.position;
            position.x = heroe2.position.x;
            transform.position = position;
        }
    }
}