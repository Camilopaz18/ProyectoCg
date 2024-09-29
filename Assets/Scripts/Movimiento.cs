using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movimiento : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float Speed;
    public float JumpForce;

    private Rigidbody2D rigidBody2d;
    private Animator animator;
    private float Horizontal;
    private bool Grounded;
    private Vector3 originalScale;
    private float LastShoot;
    private int Health = 50;



    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale; // Asigna la escala original

    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);

        animator.SetBool("running", Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;

        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();

        }


    }



    private void Jump()
    {
        rigidBody2d.AddForce(Vector2.up * JumpForce);
    }

    private void FixedUpdate()
    {
        rigidBody2d.velocity = new Vector2(Horizontal * Speed, rigidBody2d.velocity.y);
    }

    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0) Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("puas"))
        {
            Debug.Log("Muerto");
            Destroy(obj: gameObject);
        }
    }
}

