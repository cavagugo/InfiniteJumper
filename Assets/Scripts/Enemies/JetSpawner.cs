using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [Header("Dificultad / Progresión")]
    [Tooltip("A qué altura (Y de la cámara) empiezan a salir los Jets")]
    [SerializeField] private float minHeightToStart = 10f; // Ajustado a tu captura

    [Header("Recursos")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject jetPrefab;

    [Header("Tiempos")]
    [SerializeField] private float warningDuration = 2f; // Ajustado a tu captura
    [SerializeField] private float spawnInterval = 5f;

    [Header("Posición Relativa a la CÁMARA")]
    [SerializeField] private float spawnXOffset = 10f;
    [SerializeField] private float minHeightOffset = -2f; // Ajustado a tu captura
    [SerializeField] private float maxHeightOffset = 7f;  // Ajustado a tu captura

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // 1. ESPERAR ALTURA
            if (Camera.main.transform.position.y < minHeightToStart)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // 2. ESPERAR INTERVALO
            yield return new WaitForSeconds(spawnInterval);

            // 3. LANZAR ATAQUE
            if (playerTransform != null)
            {
                StartCoroutine(LaunchSequence());
            }
        }
    }

    IEnumerator LaunchSequence()
    {
        // 1. FOTO INSTANTÁNEA:
        // Guardamos dónde está la cámara EN ESTE MOMENTO EXACTO.
        // Aunque la cámara se mueva después, usaremos este valor guardado.
        float currentCamY = Camera.main.transform.position.y;

        // 2. CALCULAMOS LA ALTURA FIJA DEL ATAQUE
        float attackY = Random.Range(currentCamY + minHeightOffset, currentCamY + maxHeightOffset);

        bool comingFromRight = Random.value > 0.5f;
        int direction = comingFromRight ? -1 : 1;

        // 3. POSICIONES (En el mundo real, NO en la cámara)

        // Warning: Un poco adentro del borde
        float warningX = comingFromRight ? (spawnXOffset - 2f) : (-spawnXOffset + 2f);
        Vector3 warningPos = new Vector3(warningX, attackY, 0); // Z=0

        // Jet: Afuera del borde
        float jetX = comingFromRight ? spawnXOffset : -spawnXOffset;
        Vector3 jetSpawnPos = new Vector3(jetX, attackY, 0);

        // --- WARNING ---
        // SIN PARENT (No ponemos a la cámara como papá).
        // Se queda quieto en la posición del mundo donde nació.
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        if (warning != null) Destroy(warning);

        // --- JET ---
        // El jet nace exactamente a la misma altura 'attackY' que calculamos al principio.
        // Si el jugador saltó durante la espera, el jet pasará por debajo de él.
        GameObject jet = Instantiate(jetPrefab, jetSpawnPos, Quaternion.identity);
        JetEnemy jetScript = jet.GetComponent<JetEnemy>();
        if (jetScript != null) jetScript.Setup(direction);
    }
}