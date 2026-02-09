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

            // Desactivamos al jugador (igual que cuando cae al vacío)
            collision.gameObject.SetActive(false);

            // Opcional: Aquí podrías llamar a SceneLoader para reiniciar el nivel
            // FindObjectOfType<SceneLoader>().LoadSceneByName("NombreDeTuEscena");
        }
    }

    // Usamos también OnTrigger por si configuramos el collider como "Is Trigger"
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Debug.Log("¡Te atrapó un enemigo!");
            collision.gameObject.SetActive(false);
        }
    }
}