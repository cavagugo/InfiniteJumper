using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanish : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string vanishTrigger = "Vanish";
    [SerializeField] private float delayBeforeDisable = 0.5f; // Tiempo que dura la animación

    private Collider2D platCollider;
    private bool isVanishing = false;

    void Awake()
    {
        platCollider = GetComponent<Collider2D>();
    }

    // El Object Pool llama a OnEnable al reactivar el objeto
    void OnEnable()
    {
        // Resetear todo para que la plataforma sea funcional de nuevo
        isVanishing = false;
        if (platCollider != null) platCollider.enabled = true;

        // Regresar el Animator al estado inicial (Idle)
        if (animator != null) animator.Play("Idle", 0, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Usamos la misma lógica que tu script original para detectar el salto válido
        if (collision.relativeVelocity.y <= 0 && !isVanishing)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(VanishRoutine());
            }
        }
    }

    private IEnumerator VanishRoutine()
    {
        isVanishing = true;

        // 1. Disparar animación
        if (animator != null) animator.SetTrigger(vanishTrigger);

        // 2. Desactivar colisiones inmediatamente para que no salte dos veces
        if (platCollider != null) platCollider.enabled = false;

        // 3. Esperar a que termine el efecto visual
        yield return new WaitForSeconds(delayBeforeDisable);

        // 4. Desactivar objeto para el Object Pooling
        gameObject.SetActive(false);
    }
}
