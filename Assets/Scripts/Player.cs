using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    float movement = 0f;
    public float speed = 10f;
    Rigidbody2D rb;
    public float maxDistanceFromCameraBeforeDeath = 5f;
    private Animator animator;
    private bool facingRight = true;

    // --- Variable para ajustar dónde se teletransporta el jugador ---
    [SerializeField] private float screenBorder = 3f;

    //Variables para el PowerUp de Levitación ---
    [Header("Power Up - Levitación")]
    [SerializeField] private float levitationDuration = 10f; // Cuánto dura volando
    [SerializeField] private float levitationForce = 15f;   // Qué tan rápido sube
    private bool isLevitating = false;

    // Propiedad pública para que la moneda sepa si volamos ---
    // Esto permite leer la variable, pero no cambiarla desde fuera
    public bool IsLevitating => isLevitating;

    // Para seleccionar el input manager
    [SerializeField] private string horizontalAxis = "Horizontal";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Usa el eje personalizado
        movement = Input.GetAxis(horizontalAxis) * speed;

        // Volteamos el sprite basándonos en la dirección
        if (movement > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement < 0 && facingRight)
        {
            Flip();
        }

        // Revisar si se salió de la pantalla para teletransportarlo
        CheckScreenWrap();

        if ((transform.position.y + maxDistanceFromCameraBeforeDeath) <= Camera.main.transform.position.y)
        {
            gameObject.SetActive(false);
            if (gameManager != null) gameManager.GameOver();
            Debug.Log("Perdiste.");
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;

        // Lógica de Vuelo ---
        // Si tiene el poder activado, forzamos la velocidad hacia arriba
        if (isLevitating)
        {
            velocity.y = levitationForce;
        }

        rb.velocity = velocity;
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Función de Teletransporte
    private void CheckScreenWrap()
    {
        if (transform.position.x > screenBorder)
        {
            transform.position = new Vector3(-screenBorder, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenBorder)
        {
            transform.position = new Vector3(screenBorder, transform.position.y, transform.position.z);
        }
    }

    // --- Funciones para activar el PowerUp ---
    // Esta función la llama el script de la moneda (PickItem)
    public void ActivateLevitation()
    {
        StartCoroutine(LevitationRoutine());
    }

    // Rutina que maneja el tiempo del poder
    IEnumerator LevitationRoutine()
    {
        isLevitating = true;

        // CAMBIO DE COLOR POWERUP: Cambiar color a amarillo ---
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.color = Color.yellow;

        yield return new WaitForSeconds(levitationDuration);

        isLevitating = false;

        // --- REGRESAR AL COLOR NORMAL ---
        if (renderer != null) renderer.color = Color.white;

    }
}