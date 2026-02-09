using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Configuración de Caída")]
    [SerializeField] private float fallSpeed = 2f;      // Qué tan rápido cae hacia abajo
    [SerializeField] private float swaySpeed = 3f;      // Qué tan rápido se mueve de lado a lado (Frecuencia)
    [SerializeField] private float swayWidth = 1.5f;    // Qué tan ancho es el movimiento (Amplitud)

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        // Guardamos la posición inicial para calcular el vaivén respecto a este punto
        startPosition = transform.position;

        // Añadimos un pequeño número aleatorio para que si hay 2 bolsas, no caigan idénticas
        timeOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        // 1. Calculamos la nueva posición vertical (bajando constantemente)
        startPosition += Vector3.down * fallSpeed * Time.deltaTime;

        // 2. Calculamos el vaivén horizontal usando Seno (Mathf.Sin)
        // Esto crea un valor que oscila entre -1 y 1 suavemente
        float swayOffset = Mathf.Sin((Time.time + timeOffset) * swaySpeed) * swayWidth;

        // 3. Aplicamos la posición final combinada
        transform.position = new Vector3(startPosition.x + swayOffset, startPosition.y, transform.position.z);

        // 4. Limpieza: Si baja mucho más allá de la cámara, se destruye
        if (Camera.main != null && transform.position.y < Camera.main.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }
}