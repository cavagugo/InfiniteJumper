using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float destroyHeight = 20f; // Distancia arriba de la cámara para borrarse

    void Update()
    {
        // Moverse hacia arriba constantemente
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Opcional: Destruirse si sube demasiado (para no llenar la memoria)
        if (Camera.main != null && transform.position.y > Camera.main.transform.position.y + destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}