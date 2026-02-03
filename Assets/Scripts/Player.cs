using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    float movement = 0f;
    public float speed = 10f;
    Rigidbody2D rb;
    public float maxDistanceFromCameraBeforeDeath = 5f;
    private Animator animator;
    private bool facingRight = true; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal") * speed;

        // Volteamos el sprite basándonos en la dirección
        if (movement > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement < 0 && facingRight)
        {
            Flip();
        }

        if ((transform.position.y + maxDistanceFromCameraBeforeDeath) <= Camera.main.transform.position.y)
        {
            gameObject.SetActive(false);
            Debug.Log("Perdiste.");
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("rebotar", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //animator.SetBool("rebotar", true);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}