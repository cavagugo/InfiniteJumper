using System.Collections;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class TextBlinker : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    [SerializeField] private float totalDuration = 5f;     // Tiempo total hasta que desaparece
    [SerializeField] private float blinkSpeed = 4f;        // Frecuencia

    [Header("Ajustes de Opacidad")]
    [Range(0f, 1f)][SerializeField] private float minAlpha = 0.2f; // Opacidad mínima
    [Range(0f, 1f)][SerializeField] private float maxAlpha = 1f;   // Opacidad máxima

    private Graphic textGraphic;
    private Color originalColor;

    void OnEnable()
    {
        textGraphic = GetComponent<Graphic>();
        if (textGraphic != null)
        {
            originalColor = textGraphic.color;
            StartCoroutine(SoftBlinkRoutine());
        }
    }

    private IEnumerator SoftBlinkRoutine()
    {
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            // La fórmula crea un valor entre 0 y 1 que sube y baja suavemente
            float lerpTime = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;

            // Calculamos el alpha actual basado en nuestros límites
            float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, lerpTime);

            // Aplicamos el nuevo color con la opacidad modificada
            textGraphic.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            elapsed += Time.deltaTime;
            yield return null; // Espera al siguiente frame para máxima suavidad
        }

        // Al finalizar, desactivamos el objeto
        this.gameObject.SetActive(false);
    }

    // Aseguramos que el color se restaure si el objeto se desactiva externamente
    void OnDisable()
    {
        if (textGraphic != null)
        {
            textGraphic.color = originalColor;
        }
    }
}