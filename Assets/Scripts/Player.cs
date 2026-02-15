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
    [SerializeField] private float levitationDuration = 5f; // Cuánto dura volando
    [SerializeField] private float levitationForce = 10f;   // Qué tan rápido sube
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
            Die();
            Debug.Log("Perdiste.");
        }
    }
    public void Die()
    {
        if (gameManager != null) gameManager.GameOver();
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

        // CAMBIO DE COLOR POWERUP: Cambiar a un efecto arcoiris ---
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float tiempoPasado = 0f;
        float tiempoAviso = 1.5f;

        while (tiempoPasado < levitationDuration)
        {
            if (renderer != null)
            {
                float tiempoRestante = levitationDuration - tiempoPasado;
                float saturacion = 0.8f; // Saturación normal del arcoíris

                // Si estamos en los últimos 1.5 segundos, reducimos la saturación
                if (tiempoRestante <= tiempoAviso)
                {
                    // Calculamos un factor de 0 a 1 basado en el tiempo restante
                    float t = tiempoRestante / tiempoAviso;
                    // t va de 1 (inicio del aviso) a 0 (final del powerup)
                    saturacion = Mathf.Lerp(0f, 0.8f, t);
                }

                float hue = (tiempoPasado * 2f) % 1f;
                renderer.color = Color.HSVToRGB(hue, saturacion, 1f);
            }

            // Sumamos el tiempo de este frame y esperamos al siguiente
            tiempoPasado += Time.deltaTime;
            yield return null;
        }


        isLevitating = false;

        // --- REGRESAR AL COLOR NORMAL ---
        if (renderer != null) renderer.color = Color.white;

    }
}