using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Esto asegura que la plataforma tenga el componente de audio
public class PlatformAnimation : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioClip jumpSound; // Aquí arrastrarás tu sonido
    private AudioSource audioSource;

    [Header("Configuración de Salto")]
    [SerializeField] private bool jumpBoost = false;

    void Awake()
    {
        // Obtenemos la referencia al componente de audio automáticamente
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Checa si el jugador viene de arriba (cayendo)
        if (collision.relativeVelocity.y <= 0)
        {
            Animator anim = collision.collider.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("rebotar", true);
            }

            // --- NUEVO: Reproducir sonido ---
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Checa si el jugador viene de arriba
        if (collision.relativeVelocity.y <= 0)
        {
            Animator anim = collision.collider.GetComponent<Animator>();

            // Verificamos que anim no sea null por seguridad
            if (anim != null)
            {
                anim.SetBool("rebotar", false);
                anim.SetBool("jumpBoost", jumpBoost);
            }
        }
    }
}