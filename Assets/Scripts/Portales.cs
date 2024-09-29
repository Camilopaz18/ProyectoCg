using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portales : MonoBehaviour
{
    public GameObject player;
    public Transform destination;
    public float cooldownTime = 1f;
    private bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canTeleport)
        {
            Debug.Log("Choque");
            if (Vector2.Distance(player.transform.position, destination.transform.position) > 0.03f)
            {
                // player.transform.position = destination.transform.position;
                StartCoroutine(TeleportPlayer());
            }
        }
    }
    private IEnumerator TeleportPlayer()
    {
        canTeleport = false;


        player.transform.position = destination.transform.position;


        GetComponent<Collider2D>().enabled = false;


        yield return new WaitForSeconds(cooldownTime);


        GetComponent<Collider2D>().enabled = true;
        canTeleport = true;
    }




    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
