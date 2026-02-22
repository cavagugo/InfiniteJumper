using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si chocamos con el Player
        if (collision.gameObject.GetComponent<Player>())
        {
            Debug.Log("¡Te atrapó un enemigo!");

            GameManager gm = FindObjectOfType<GameManager>();
            gm.GameOver();
            // Desactivamos al jugador
            //collision.gameObject.SetActive(false);
        }
    }

    // Usamos también OnTrigger por si configuramos el collider como "Is Trigger"
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.GameOver();

            Debug.Log("¡Te atrapó un enemigo!");
            //collision.gameObject.SetActive(false);
            

        }
    }
}