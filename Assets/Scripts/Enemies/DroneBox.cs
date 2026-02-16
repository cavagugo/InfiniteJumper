using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DroneBox : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        // Se destruye la caja después de 5 segundos para que no se llene la memoria
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡La caja golpeó al jugador!");

            // Desactiva al jugador
            collision.gameObject.SetActive(false);

            // Busca al GameManager para avisar que perdiste
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.GameOver();
            }

            Destroy(gameObject);
        }
        // Si choca con el suelo
        else if (collision.CompareTag("Platform"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;

                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            if (anim != null) anim.Play("Box_hit");
            Destroy(gameObject,0.3f); // La caja se rompe al tocar el piso
        }
    }
}