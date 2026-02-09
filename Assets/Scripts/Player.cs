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

    //Para seleccionar el input manager
    [SerializeField] private string horizontalAxis = "Horizontal"; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
}