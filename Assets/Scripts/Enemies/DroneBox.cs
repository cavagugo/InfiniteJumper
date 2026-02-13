using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DroneBox : MonoBehaviour
{
    private void Start()
    {
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
        // Si choca con el suelo (suponiendo que las plataformas tienen tag "Ground")
        else if (collision.CompareTag("Ground") || collision.CompareTag("Platform"))
        {
            Destroy(gameObject); // La caja se rompe al tocar el piso
        }
    }
}