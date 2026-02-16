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

    [Tooltip("Opcional: Arrastra aquí un prefab de partículas de explosión")]
    [SerializeField] private GameObject popEffect;

    void Start()
    {
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

        // 1. Si pusiste un efecto (partículas), lo creamos donde está el globo
        if (popEffect != null)
        {
            Instantiate(popEffect, transform.position, Quaternion.identity);
        }

        // 2. Opcional: Sonido de "PUM"
        // AudioSource.PlayClipAtPoint(popSound, transform.position);

        // 3. Destruimos el globo
        Destroy(gameObject);
    }
}