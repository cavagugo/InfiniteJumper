using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JetEnemy : MonoBehaviour
{
    [Header("Ajustes del Jet")]
    [SerializeField] private float speed = 25f; // Velocidad Jet
    private int direction = 1; // 1 = Derecha, -1 = Izquierda

    // Esta función la llamará el Spawner cuando cree el jet
    public void Setup(int dir)
    {
        direction = dir;

        // Si va hacia la izquierda, volteamos el sprite
        if (direction == -1)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        // Se destruye después de 3 segundos por si acaso no sale de pantalla
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Movimiento rectilíneo a toda velocidad
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Si sale de la pantalla, se destruye
        if (Mathf.Abs(transform.position.x) > 25f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El Jet impactó al jugador!");
            collision.gameObject.SetActive(false);

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.GameOver();
        }
    }
}