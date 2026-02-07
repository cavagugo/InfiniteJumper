using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Checa si el jugador viene de arriba
        if (collision.relativeVelocity.y <= 0)
        {
            //Hace que el jugador salte con una fuerza constante sin importar desde qué tan alto o bajo caiga
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            //Animator anim = collision.collider.GetComponent<Animator>();
            if (rb != null)
            {
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;

                //anim.SetBool("rebotar", true);
            }
        }
        
    }
}
