using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float timeToDrop = 3f;

    [Header("Movimiento")]
    [SerializeField] private float followSpeedX = 4f; // Velocidad lateral
    [SerializeField] private float hoverHeight = 4f;  // Altura deseada
    [SerializeField] private float verticalSmoothness = 2f; // Que tan rapido sigue tu salto (Menor = más lento/pesado)

    [Header("Escape")]
    [SerializeField] private float escapeSpeed = 8f; // Velocidad al huir

    private Transform playerTransform;
    private float timer = 0f;
    private bool hasDropped = false;
    private int escapeDirection = 0; // 1 derecha, -1 izquierda
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null) anim.Play("Dron_withbox");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        // Asegurar que la física no interfiera
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Si ya soltó la caja este se ira
        if (hasDropped)
        {
            FlyAway();
            return;
        }

        // Si no ha soltado la caja seguira al jugador
        if (playerTransform != null)
        {
            StalkPlayer();

            // Cuenta regresiva
            timer += Time.deltaTime;
            if (timer >= timeToDrop)
            {
                DropBox();
            }
        }
    }

    void StalkPlayer()
    {
        // 1. EJE X: Perseguir al jugador normalmente
        float targetX = Mathf.MoveTowards(transform.position.x, playerTransform.position.x, followSpeedX * Time.deltaTime);

        // 2. EJE Y: Usamos Lerp para que el cambio de altura sea progresivo, no instantáneo
        float targetY = playerTransform.position.y + hoverHeight;
        float smoothY = Mathf.Lerp(transform.position.y, targetY, verticalSmoothness * Time.deltaTime);

        transform.position = new Vector3(targetX, smoothY, transform.position.z);
    }

    void DropBox()
    {
        if (boxPrefab != null)
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
        }

        hasDropped = true;
        if (anim != null) anim.Play("Dron_nobox");
        Debug.Log("¡Caja fuera! Iniciando escape...");

        // Decidir hacia dónde huir (hacia el borde más cercano)
        if (transform.position.x > 0)
            escapeDirection = 1; // Está a la derecha, se va a la derecha
        else
            escapeDirection = -1; // Está a la izquierda, se va a la izquierda

    }

    void FlyAway()
    {
        // Moverse rápido hacia el lado elegido y un poco hacia arriba
        transform.Translate(new Vector2(escapeDirection, 0.5f) * escapeSpeed * Time.deltaTime);

        // Si se aleja mucho se destruye
        if (Mathf.Abs(transform.position.x) > 20f)
        {
            Destroy(gameObject);
        }
    }
}