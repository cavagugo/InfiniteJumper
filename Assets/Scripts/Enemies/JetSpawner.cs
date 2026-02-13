using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [Header("Dificultad / Progresión")]
    [Tooltip("A qué altura (Y de la cámara) empiezan a salir los Jets")]
    [SerializeField] private float minHeightToStart = 50f; // Decide a que altura de la camara iniciara(opcional)

    [Header("Recursos")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject jetPrefab;

    [Header("Tiempos")]
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Posición Relativa a la CÁMARA")]
    [SerializeField] private float spawnXOffset = 10f;
    [SerializeField] private float minHeightOffset = -4f;
    [SerializeField] private float maxHeightOffset = 4f;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        // Esperamos un pequeño tiempo inicial para que cargue todo
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // Si la cámara aún está muy abajo, espera y no se hace nada.
            if (Camera.main.transform.position.y < minHeightToStart)
            {
                // Espera 1 segundo y vuelve a checar
                yield return new WaitForSeconds(1f);
                continue; // "Continue" salta al siguiente ciclo del while sin ejecutar lo de abajo
            }

            // Si ya paso la altura, se espera al intervalo de ataque
            yield return new WaitForSeconds(spawnInterval);

            // Ataca solo si el jugador sigue existiendo
            if (playerTransform != null)
            {
                StartCoroutine(LaunchSequence());
            }
        }
    }

    IEnumerator LaunchSequence()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        float randomY = Random.Range(cameraPos.y + minHeightOffset, cameraPos.y + maxHeightOffset);

        bool comingFromRight = Random.value > 0.5f;
        int direction = comingFromRight ? -1 : 1;

        // Posiciones
        float warningX = comingFromRight ? (spawnXOffset - 2f) : (-spawnXOffset + 2f);
        Vector3 warningPos = new Vector3(warningX, randomY, 0);

        float jetX = comingFromRight ? spawnXOffset : -spawnXOffset;
        Vector3 jetSpawnPos = new Vector3(jetX, randomY, 0);

        // --- WARNING ---
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
        yield return new WaitForSeconds(warningDuration);
        Destroy(warning);

        // --- JET ---
        GameObject jet = Instantiate(jetPrefab, jetSpawnPos, Quaternion.identity);
        JetEnemy jetScript = jet.GetComponent<JetEnemy>();
        if (jetScript != null) jetScript.Setup(direction);
    }
}