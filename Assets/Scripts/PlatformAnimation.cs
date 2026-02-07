using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    //[SerializeField] private string boolName;
    [SerializeField] private bool jumpBoost = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Checa si el jugador viene de arriba
        if (collision.relativeVelocity.y <= 0)
        {
            Animator anim = collision.collider.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("rebotar", true);
            }
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        //Checa si el jugador viene de arriba
        if (collision.relativeVelocity.y <= 0)
        {
            Animator anim = collision.collider.GetComponent<Animator>();

            anim.SetBool("rebotar", false);
            anim.SetBool("jumpBoost", jumpBoost);
        }
    }
}
