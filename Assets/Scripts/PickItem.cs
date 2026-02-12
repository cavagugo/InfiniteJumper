using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int coinsForPowerUp = 15; // Meta para activar el poder

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Reproduce un sonido, desactiva la moneda y aumenta en 1 el contador si colisionó con un player   
        if (collision.gameObject.tag == "Player")
        {
            // Primero obtenemos el script del Player
            Player playerScript = collision.GetComponent<Player>();

            // --- BLOQUEO DE SEGURIDAD ---
            // Si el jugador YA está levitando, ignoramos esta moneda por completo.
            // No suena, no suma puntos y no desaparece.
            if (playerScript != null && playerScript.IsLevitating)
            {
                return;
            }

            audioSource.Play();

            // Aumentar moneda
            GlobalVariables.coins++;

            // --- Checar PowerUp ---
            if (GlobalVariables.coins >= coinsForPowerUp)
            {
                GlobalVariables.coins = 0; // Resetear contador

                if (playerScript != null)
                {
                    playerScript.ActivateLevitation();
                    Debug.Log("¡PowerUp Activado!");
                }
            }

            gameObject.SetActive(false);
        }
    }
}