using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private float deathDelay = 2.0f;

    public void GameOver()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        GlobalVariables.gameOver = true;
        if (BGM != null) BGM.Stop();
        if (deathSFX != null) deathSFX.Play();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            // --- PASO 1: REPOSICIONAMIENTO (Si murió fuera de pantalla por abajo) ---
            float camY = Camera.main.transform.position.y;
            float camHeight = Camera.main.orthographicSize;
            float lowerBound = camY - camHeight;

            // Si el jugador está por debajo del límite visible, lo subimos un poquito
            if (player.transform.position.y < lowerBound)
            {
                player.transform.position = new Vector3(player.transform.position.x, lowerBound + 0.5f, player.transform.position.z);
            }

            // --- PASO 2: CONGELAR FÍSICAS ---
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }

            // Desactivamos colisiones de inmediato
            if (player.TryGetComponent<Collider2D>(out Collider2D col))
                col.enabled = false;

            // --- PASO 3: ACTIVAR PARTÍCULAS PRIMERO ---
            ParticleSystem deathParticles = player.GetComponentInChildren<ParticleSystem>();
            if (deathParticles != null)
            {
                // Opcional: Desvincular del padre para que el salto sea libre
                deathParticles.transform.SetParent(null);
                deathParticles.Play();
            }
        }

        // --- PASO 4: EL "FRAME DE GRACIA" ---
        // Esperamos un instante minúsculo para que la partícula aparezca sobre el jugador
        yield return new WaitForSeconds(0.05f);

        // --- PASO 5: DESACTIVAR VISUALES ---
        // Ahora que la partícula ya tapó al jugador, ocultamos el sprite
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
                sprite.enabled = false;
        }

        // Esperamos a que la partícula haga su animación completa
        yield return new WaitForSeconds(deathDelay);

        // FINALIZAR
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
    }
}