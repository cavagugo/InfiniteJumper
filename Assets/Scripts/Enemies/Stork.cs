using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stork : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 6f;

    private int direction = 1; // 1 = Derecha, -1 = Izquierda

    void Start()
    {
        // 1. DETECTAR LADO DE APARICIÓN
        if (transform.position.x > 0)
        {
            // Está a la derecha, quiere ir a la izquierda
            direction = -1;

            // Volteamos el gráfico
            Vector3 newScale = transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x); // Asegura que sea negativo
            transform.localScale = newScale;
        }
        else
        {
            // Está a la izquierda, quiere ir a la derecha
            direction = 1;

            // Aseguramos que mire a la derecha (positivo)
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }

        // 2. MUERTE AUTOMÁTICA
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Usamos 'Space.World' para movernos usando las coordenadas del mapa,
        // ignorando si el sprite está volteado o no.
        // Y multiplicamos explícitamente por 'direction' (-1 o 1).

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);
    }

    // ... (Tu Flip y OnTriggerEnter se quedan igual o puedes borrar Flip si usas la lógica del Start de arriba)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.GameOver();
        }
    }
}