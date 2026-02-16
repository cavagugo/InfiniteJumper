using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float destroyHeight = 20f; // Seguridad por si se aleja mucho

    [Header("Explosión por Tiempo")]
    [Tooltip("Segundos que dura el globo antes de explotar solo")]
    [SerializeField] private float lifeTime = 8f;
    [SerializeField] private AudioSource audioSource;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Iniciamos la cuenta regresiva para explotar
        StartCoroutine(PopBalloonRoutine());
    }

    void Update()
    {
        // 1. MOVERSE HACIA ARRIBA
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // 2. SEGURIDAD: Si se aleja muchísimo de la cámara (por si el tiempo falla), se borra
        if (Camera.main != null && transform.position.y > Camera.main.transform.position.y + destroyHeight)
        {
            Destroy(gameObject);
        }
    }

    // Esta rutina espera el tiempo y luego explota el globo
    IEnumerator PopBalloonRoutine()
    {
        // Esperamos X segundos
        yield return new WaitForSeconds(lifeTime);

        // --- MOMENTO DE LA EXPLOSIÓN ---

        if (anim != null)
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            anim.Play("Balloon_pop");
            
        }

        audioSource.Play();

        //Destruimos el globo
        Destroy(gameObject, 0.5f);
    }
}