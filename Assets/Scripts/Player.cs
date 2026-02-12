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

    // Variable para ajustar dónde se teletransporta el jugador ---
    // (Esto evita conflictos con el GlobalVariables que usa el generador de nivel)
    [SerializeField] private float screenBorder = 3f;
    // ----------------------------------------------------------------------

    //Para seleccionar el input manager
    //HorizontalP1 es AD y HorizontalP2 es flechas del teclado
    [SerializeField] private string horizontalAxis = "Horizontal";

    // Start is called before the first frame update
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

        // Revisar si se salió de la pantalla para teletransportarlo ---
        CheckScreenWrap();

        if ((transform.position.y + maxDistanceFromCameraBeforeDeath) <= Camera.main.transform.position.y)
        {
            gameObject.SetActive(false);
            gameManager.GameOver();
            Debug.Log("Perdiste.");
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Función de Teletransporte ---
    private void CheckScreenWrap()
    {
        // Si sale por la derecha, aparece en la izquierda
        if (transform.position.x > screenBorder)
        {
            transform.position = new Vector3(-screenBorder, transform.position.y, transform.position.z);
        }
        // Si sale por la izquierda, aparece en la derecha
        else if (transform.position.x < -screenBorder)
        {
            transform.position = new Vector3(screenBorder, transform.position.y, transform.position.z);
        }
    }
}