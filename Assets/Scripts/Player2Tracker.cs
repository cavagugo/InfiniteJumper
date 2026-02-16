using UnityEngine;

public class Player2Tracker : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player2; // Arrastra al Jugador 2 aquí

    [Header("Configuración")]
    [SerializeField] private float yOffset = 0;       // Altura fija relativa a la cámara
    [SerializeField] private float showThreshold = 0f; // Margen extra por si quieres que aparezca un poco después

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtenemos el renderizador para ocultarlo/mostrarlo sin apagar el script
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Posicionamos el objeto inicialmente en su yOffset
        transform.localPosition = new Vector3(transform.localPosition.x, yOffset, 10);
    }

    void Update()
    {
        if (player2 == null) return;

        // Comprobamos si el jugador 2 está más arriba que la posición actual de la flecha
        // Usamos transform.position.y (global) para comparar alturas reales
        if (player2.position.y > (transform.position.y + showThreshold))
        {
            // Mostramos el sprite
            if (spriteRenderer != null) spriteRenderer.enabled = true;

            // Calculamos la X relativa al centro de la cámara
            float targetX = player2.position.x - transform.parent.position.x;

            // Actualizamos la posición local
            // Mantenemos yOffset para que no se mueva de su carril superior
            transform.localPosition = new Vector3(targetX, yOffset, 10);
        }
        else
        {
            // Ocultamos el sprite si el jugador está por debajo de la flecha
            if (spriteRenderer != null) spriteRenderer.enabled = false;
        }
    }
}