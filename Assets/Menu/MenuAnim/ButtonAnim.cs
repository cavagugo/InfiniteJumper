using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonAnim : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator anim;
    private AudioSource audioSource;

    [Header("Sonidos")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    void Awake()
    {
        // Si no se asigna en el inspector, buscarlo en el mismo objeto
        if (anim == null) anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        // Configuramos el AudioSource para que no se escuche al iniciar
        audioSource.playOnAwake = false;
    }

    // Esta función se llama cuando el mouse entra (Hover)
    public void SetHover(bool parametro)
    {
        anim.SetBool("hover", parametro);

        // Solo reproducimos el sonido si parametro es true (cuando entra el mouse, no cuando sale)
        if (parametro && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // Esta función se llama al presionar (Click)
    public void SetPressed(bool parametro)
    {
        anim.SetBool("pressed", parametro);

        // Reproducimos sonido al presionar (cuando parametro es true)
        if (parametro && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

}